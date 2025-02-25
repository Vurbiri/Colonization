//Assets\Colonization\Scripts\EntryPoint\Project\LogOnPanel.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class LogOnPanel : MonoBehaviour
    {
        private WaitResult<bool> _waitLogOn;
        private YandexSDK _ysdk;

        public IEnumerator TryLogOn_Cn(YandexSDK ysdk)
        {
            gameObject.SetActive(true);

            _ysdk = ysdk;
            _waitLogOn = new();
            
            bool resultAuthorization = false;
            while (true)
            {
                yield return _waitLogOn;

                if (!_waitLogOn.Result)
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
            _waitLogOn.SetResult(true);
        }

        private void Update()
        {
            if (_ysdk.IsLogOn)
                _waitLogOn.SetResult(true);
        }
    }
}
