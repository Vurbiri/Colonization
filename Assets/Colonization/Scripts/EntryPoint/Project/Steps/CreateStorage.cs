//Assets\Colonization\Scripts\EntryPoint\Project\Steps\CreateStorage.cs
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
        private readonly YandexSDK _ysdk;
        private readonly Settings _settings;
        private ProjectStorage _projectStorage;

        public CreateStorage(DIContainer diContainer, Coroutines coroutine, ILoadingScreen loadingScreen, LogOnPanel logOnPanel) : base("StorageCreationStep")
        {
            _diContainer = diContainer;
            _coroutine = coroutine;
            _loadingScreen = loadingScreen;
            _logOnPanel = logOnPanel;
            _ysdk = _diContainer.Get<YandexSDK>();
            _settings = _diContainer.Get<Settings>();
        }

        public override IEnumerator GetEnumerator()
        {
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
            yield return _coroutine.Run(CreateStorageService_Cn());
            _settings.Init(_ysdk, _projectStorage);
        }

        private IEnumerator CreateStorageService_Cn()
        {
            if (Create(out IStorageService storage))
            {
                bool result = false;
                yield return storage.Load_Cn((b) => result = b);
                Message.Log(result ? "Сохранения загружены" : "Сохранения не найдены");
            }
            else
            {
                Message.Log("StorageService не определён");
            }

            _diContainer.ReplaceInstance(storage);
            _diContainer.ReplaceInstance(_projectStorage = new(storage));

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
