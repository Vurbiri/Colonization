//Assets\Colonization\Scripts\EntryPoint\Project\LogOnPanel.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    public class LogOnPanel : MonoBehaviour
    {
        [SerializeField] private LanguageSwitch _languageSwitch;

        private ProjectStorage _storage;
        private WaitResultSource<bool> _waitLogOn;
        private YandexSDK _ysdk;

        public IEnumerator TryLogOn_Cn(YandexSDK ysdk, Settings settings, ProjectStorage storage)
        {
            _languageSwitch.Init(settings);

            gameObject.SetActive(true);

            _storage = storage;
            _ysdk = ysdk;
            _waitLogOn = new();
            
            bool resultAuthorization = false;
            while (true)
            {
                yield return _waitLogOn;

                if (!_waitLogOn.Value)
                    break;

                yield return StartCoroutine(ysdk.Authorization_Cn((b) => resultAuthorization = b));
                if (resultAuthorization)
                    break;

                _waitLogOn = new();
                //Message.BannerKey("ErrorLogon", MessageType.Error);
            }

            gameObject.SetActive(false);
        }

        public void OnGuest()
        {
            _waitLogOn.SetResult(false);
        }

        public void OnLogOn()
        {
            _storage.Save();
            _waitLogOn.SetResult(true);
        }

        private void FixedUpdate()
        {
            if (_ysdk != null && _ysdk.IsLogOn) OnLogOn();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            //EUtility.SetPrefab(ref _langPrefab);

            if (_languageSwitch == null)
                _languageSwitch = GetComponentInChildren<LanguageSwitch>();
        }
#endif
    }
}
