using System.Collections;
using UnityEngine;

namespace Vurbiri.Yandex
{
    sealed public class JsonToYandex : AStorageOneFile
    {
        private readonly YandexSDK _ysdk;

        public override bool IsValid => Application.platform == RuntimePlatform.WebGLPlayer && _ysdk != null && _ysdk.IsLogOn;

        public JsonToYandex(string key, MonoBehaviour monoBehaviour, YandexSDK ysdk) : base(key, monoBehaviour)
        {
            _ysdk = ysdk;
        }

        protected override IEnumerator LoadFromFile_Cn()
        {
            var wait = _ysdk.Load(_key);
            yield return wait;
            _outputJson = wait.Value;
        }
        protected override IEnumerator SaveToFile_Cn()
        {
            var wait = _ysdk.Save(_key, Serialize(_saved));
            yield return wait;
            _outputResult = wait.Value;
        }

    }
}
