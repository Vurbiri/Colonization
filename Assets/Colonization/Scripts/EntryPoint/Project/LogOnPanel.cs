#if YSDK

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

			while (true)
			{
                gameObject.SetActive(true);

                yield return _waitLogOn.Restart();
				if (!_waitLogOn.Value)
					break;

				yield return ysdk.Authorization_Cn(Out<bool>.Get(out int key));
				if (Out<bool>.Result(key))
					break;

				Banner.Open(Localization.Instance.GetText(_errorLogon), MessageTypeId.Error, 3f);
			}

			Destroy(transform.parent.gameObject);
		}

        public void OnLogOn() => OnClock(true);

        public void OnGuest() => OnClock(false);

        private void FixedUpdate()
		{
			if (_ysdk != null && _ysdk.IsLogOn) OnClock(true);
        }

		private void OnClock(bool value)
		{
            _storage.Save();
            _waitLogOn.Set(value);
            gameObject.SetActive(false);
        }
	}
}

#endif