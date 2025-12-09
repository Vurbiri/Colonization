using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.International;
using Vurbiri.UI;
using Vurbiri.Yandex;

namespace Vurbiri.Colonization
{
    public class LogOnPanel : MonoBehaviour
    {
        [SerializeField] private FileIdAndKey _errorLogon;

        private readonly WaitResultSource<bool> _waitLogOn = new();
        private ProjectStorage _storage;
        private YandexSDK _ysdk;

        public IEnumerator TryLogOn_Cn(YandexSDK ysdk, ProjectStorage storage)
        {
            _storage = storage;
            _ysdk = ysdk;

            gameObject.SetActive(true);

            while (true)
            {
                yield return _waitLogOn.Restart();
                if (!_waitLogOn.Value)
                    break;

                yield return StartCoroutine(ysdk.Authorization_Cn(Out<bool>.Get(out int key)));
                if (Out<bool>.Result(key))
                    break;

                Banner.Open(Localization.Instance.GetText(_errorLogon), MessageTypeId.Error, 3f);
            }

            gameObject.SetActive(false);
        }

        public void OnGuest()
        {
            _waitLogOn.Set(false);
        }

        public void OnLogOn()
        {
            _storage.Save();
            _waitLogOn.Set(true);
        }

        private void FixedUpdate()
        {
            if (_ysdk != null && _ysdk.IsLogOn) OnLogOn();
        }
    }
}
