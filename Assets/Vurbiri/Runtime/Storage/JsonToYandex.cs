using UnityEngine;

namespace Vurbiri
{
    sealed public class JsonToYandex : AStorageOneFile
    {
        private readonly YandexSDK _ysdk;

        public override bool IsValid => _ysdk != null && _ysdk.IsLogOn;

        public JsonToYandex(string key, MonoBehaviour monoBehaviour, YandexSDK ysdk) : base(key, monoBehaviour)
        {
            _ysdk = ysdk;
        }

        protected override WaitResult<string> LoadFromFile_Wait() => _ysdk.Load(_key);
        protected override WaitResult<bool> SaveToFile_Wait() => _ysdk.Save(_key, Serialize(_saved));

    }
}
