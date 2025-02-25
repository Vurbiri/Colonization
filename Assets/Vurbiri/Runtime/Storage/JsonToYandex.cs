//Assets\Vurbiri\Runtime\Storage\JsonToYandex.cs
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    public class JsonToYandex : ASaveLoadJsonTo
    {
        private YandexSDK _ysdk;

        public override bool IsValid => _ysdk != null && _ysdk.IsLogOn;

        public override bool Init(IReadOnlyDIContainer container)
        {
            _ysdk = container.Get<YandexSDK>();
            return _ysdk.IsLogOn;
        }

        public override IEnumerator Load_Cn(string key, Action<bool> callback)
        {
            _key = key;

            WaitResult<string> waitResult;
            string json;

            yield return (waitResult = _ysdk.Load(_key));
            json = waitResult.Result;

            if (!string.IsNullOrEmpty(json))
            {
                Return<Dictionary<string, string>> d = Deserialize<Dictionary<string, string>>(json);

                if (d.Result)
                {
                    _saved = d.Value;
                    callback?.Invoke(true);
                    yield break;
                }
            }

            _saved = new();
            callback?.Invoke(false);
        }

        protected override WaitResult<bool> SaveToFile_Wt() => _ysdk.Save(_key, Serialize(_saved));
    }
}
