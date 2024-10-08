using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    public class JsonToYandex : ASaveLoadJsonTo
    {
        private readonly YandexSDK _ysdk;

        public override bool IsValid => _ysdk.IsLogOn;

        public JsonToYandex()
        {
            _ysdk = YandexSDK.Instance;
        }

        public override IEnumerator Init_Coroutine(string key, Action<bool> callback)
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

        protected override WaitResult<bool> SaveToFile_Wait() => _ysdk.Save(_key, Serialize(_saved));
    }
}
