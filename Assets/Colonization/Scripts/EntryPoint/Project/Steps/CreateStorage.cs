using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreateStorage : ALocalizationLoadingStep
    {
        private readonly DIContainer _diContainer;
        private readonly Coroutines _coroutine;
        private readonly ILoadingScreen _loadingScreen;
        private readonly LogOnPanel _logOnPanel;
        private YandexSDK _ysdk;
        private Settings _settings;
        private ProjectStorage _projectStorage;

        public CreateStorage(DIContainer diContainer, Coroutines coroutine, ILoadingScreen loadingScreen, LogOnPanel logOnPanel) : base("StorageCreationStep")
        {
            _diContainer = diContainer;
            _coroutine = coroutine;
            _loadingScreen = loadingScreen;
            _logOnPanel = logOnPanel;
        }

        public override IEnumerator GetEnumerator()
        {
            _ysdk = _diContainer.Get<YandexSDK>();
            _settings = _diContainer.Get<Settings>();

            yield return _coroutine.Run(CreateStorage_Cn());
            if (!_ysdk.IsLogOn)
            {
                yield return _coroutine.Run(_loadingScreen.SmoothOff());
                yield return _coroutine.Run(_logOnPanel.TryLogOn_Cn(_ysdk, _settings, _projectStorage));
                yield return _coroutine.Run(_loadingScreen.SmoothOn());
                if (_ysdk.IsLogOn)
                    yield return _coroutine.Run(CreateStorage_Cn());
            }
        }

        private IEnumerator CreateStorage_Cn()
        {
            if (Create(out IStorageService storage))
            {
                bool result = false;
                yield return storage.Load_Cn((b) => result = b);
                Message.Log(result ? "Сохранение загружено" : "Сохранение не найдено");
            }
            else
            {
                Message.Log("StorageService не определён");
            }

            _diContainer.ReplaceInstance(storage);
            _diContainer.ReplaceInstance(_projectStorage = new(storage));

            _settings.Init(_ysdk, _projectStorage);

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
                MonoBehaviour monoBehaviour = _coroutine;

                yield return new JsonToYandex(SAVE_KEYS.PROJECT, monoBehaviour, _ysdk);
                yield return new JsonToLocalStorage(SAVE_KEYS.PROJECT, monoBehaviour);
                yield return new JsonToCookies(SAVE_KEYS.PROJECT, monoBehaviour);
            }
            #endregion
        }

    }
}
