using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreateStorage : ALocalizationLoadingStep
    {
        private readonly ProjectContent _content;
        private readonly MonoBehaviour _mono;
        private readonly ILoadingScreen _loadingScreen;
        private readonly LogOnPanel _logOnPanel;

        public CreateStorage(ProjectContent content, MonoBehaviour mono, ILoadingScreen loadingScreen, LogOnPanel logOnPanel) : base("StorageCreationStep")
        {
            IStorageService.Init();

            _content = content;
            _mono = mono;
            _loadingScreen = loadingScreen;
            _logOnPanel = logOnPanel;
        }

        public override IEnumerator GetEnumerator()
        {
            yield return _mono.StartCoroutine(CreateStorage_Cn());
            if (!_content.ysdk.IsLogOn)
            {
                yield return _mono.StartCoroutine(_loadingScreen.SmoothOff());
                yield return _mono.StartCoroutine(_logOnPanel.TryLogOn_Cn(_content.ysdk, _content.settings, _content.projectStorage));
                yield return _mono.StartCoroutine(_loadingScreen.SmoothOn());
                if (_content.ysdk.IsLogOn)
                    yield return _mono.StartCoroutine(CreateStorage_Cn());
            }
        }

        private IEnumerator CreateStorage_Cn()
        {
            if (Create(out IStorageService storage))
            {
                bool result = false;
                yield return storage.Load_Cn((b) => result = b);
                Log.Info(result ? "Сохранение загружено" : "Сохранение не найдено");
            }
            else
            {
                Log.Info("StorageService не определён");
            }

            _content.storageService = storage;
            _content.projectStorage = new(storage);

            _content.settings.Init(_content.ysdk, _content.projectStorage);

            #region Local: Create(..), Creator()
            // =====================
            bool Create(out IStorageService storage)
            {
                var creator = Creator();
                while (creator.MoveNext())
                {
                    storage = creator.Current;
                    if (storage.IsValid)
                        return true;
                }

                storage = new EmptyStorage();
                return storage.IsValid;
            }
            // =====================
            IEnumerator<IStorageService> Creator()
            {
                var monoBehaviour = _mono;

                yield return new JsonToYandex(SAVE_KEYS.PROJECT, monoBehaviour, _content.ysdk);
                yield return new JsonToLocalStorage(SAVE_KEYS.PROJECT, monoBehaviour);
                yield return new JsonToCookies(SAVE_KEYS.PROJECT, monoBehaviour);
            }
            #endregion
        }

    }
}
