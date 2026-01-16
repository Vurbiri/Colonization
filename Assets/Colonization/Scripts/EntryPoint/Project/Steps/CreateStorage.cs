using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.EntryPoint;
using Vurbiri.Storage;
using Vurbiri.Web;

namespace Vurbiri.Colonization.EntryPoint
{
	sealed internal class CreateStorage : ALocalizationLoadingStep
	{
		private readonly ProjectContent _content;
		private readonly MonoBehaviour _mono;
		private readonly ILoadingScreen _loadingScreen;

#if YSDK
        private readonly Canvas _logOnPanel;

		public CreateStorage(ProjectContent content, MonoBehaviour mono, ILoadingScreen loadingScreen, Canvas logOnPanel) : base("StorageCreationStep")
		{
			_content = content;
			_mono = mono;
			_loadingScreen = loadingScreen;
			_logOnPanel = logOnPanel;
		}
#else
		public CreateStorage(ProjectContent content, MonoBehaviour mono, ILoadingScreen loadingScreen) : base("StorageCreationStep")
		{
			_content = content;
			_mono = mono;
			_loadingScreen = loadingScreen;
		}
#endif

		public override IEnumerator GetEnumerator()
		{
			yield return _mono.StartCoroutine(CreateStorage_Cn());
#if YSDK
            if (!_content.ysdk.IsLogOn)
			{
				var logOnPanel = Object.Instantiate(_logOnPanel).GetComponentInChildren<LogOnPanel>(true);
				yield return null;

                _mono.StartCoroutine(_loadingScreen.SmoothOff());

                yield return _mono.StartCoroutine(logOnPanel.TryLogOn_Cn(_content.ysdk, _content.projectStorage));
				yield return _mono.StartCoroutine(_loadingScreen.SmoothOn());
				if (_content.ysdk.IsLogOn)
					yield return _mono.StartCoroutine(CreateStorage_Cn());
			}
#endif
        }

        private IEnumerator CreateStorage_Cn()
		{
			if (Create(out IStorageService storage))
			{
				yield return storage.Load_Cn(Out<bool>.Get(out int key));
				if (Out<bool>.Result(key))
					Log.Info("[StorageService] Save is loaded");
				else
					Log.Info("[StorageService] No save found");
			}

			_content.storageService = storage;
			_content.projectStorage = new(storage);

#if YSDK
			_content.settings.Init(_content.ysdk, _content.projectStorage);
#else
			_content.settings.Init(_content.projectStorage);
#endif

			#region Local: Create(..), Creator()
			// =====================
			bool Create(out IStorageService storage)
			{
				storage = new EmptyStorage();
				var creator = Creator();

				while (creator.MoveNext())
				{
					if (creator.Current.IsValid)
					{
						storage = creator.Current;
						break;
					}
				}

				Log.Info($"[StorageService] Creating storage: {storage.GetType().Name}");
				return storage.IsValid;
			}
			// =====================
			IEnumerator<IStorageService> Creator()
			{
				var monoBehaviour = _mono;
#if YSDK && !UNITY_EDITOR
				yield return new JsonToYandex(SAVE_KEYS.PROJECT, monoBehaviour, _content.ysdk);
#endif
				yield return new JsonToLocalStorage(SAVE_KEYS.PROJECT, monoBehaviour);
				yield return new JsonToCookies(SAVE_KEYS.PROJECT, monoBehaviour);
				yield return new JsonToHard(SAVE_KEYS.PROJECT, monoBehaviour);
			}
			#endregion
		}

	}
}
