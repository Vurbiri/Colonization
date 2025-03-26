//Assets\Vurbiri\Runtime\Storage\JsonToYandex.cs
using UnityEngine;

namespace Vurbiri
{
    sealed public class JsonToYandex : ASaveLoadJsonTo
    {
        private readonly YandexSDK _ysdk;

        public override bool IsValid => _ysdk != null && _ysdk.IsLogOn;

        public JsonToYandex(MonoBehaviour monoBehaviour, YandexSDK ysdk) : base(monoBehaviour)
        {
            _ysdk = ysdk;
        }

        protected override WaitResult<string> LoadFromFile_Wait() => _ysdk.Load(_key);
        protected override WaitResult<bool> SaveToFile_Wait() => _ysdk.Save(_key, Serialize(_saved));

    }
}
