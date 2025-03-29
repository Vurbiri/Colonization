//Assets\Colonization\Scripts\EntryPoint\Project\ProjectInitialization.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;
using Vurbiri.Colonization.UI;
using Vurbiri.TextLocalization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectInitialization : MonoBehaviour
    {
        [SerializeField] private LoadScene _startScene;
        [Space]
        [SerializeField] private string _projectStorageKey = SAVE_KEYS.PROJECT;
        [Space]
        [SerializeField] private LogOnPanel _logOnPanel;
        [Space]
        [SerializeField] private EnumArray<Files, bool> _localizationFiles = new(false);
        [Space]
        [SerializeField] private string _leaderboardName = "lbColonization";
        [Space]
        [SerializeField] private TextColorSettingsScriptable _settingsColorScriptable;
        [Space]
        [SerializeField] private Settings _settings;

        private Coroutines _coroutine;
        private YandexSDK _ysdk;
        private ProjectStorage _projectStorage;

        public IEnumerator Init_Cn(DIContainer diContainer, LoadingScreen loadingScreen)
        {
            _startScene.Start();

            //----------------------------------
            Message.Log("Start Init Project");

            diContainer.AddInstance(Localization.Instance).SetFiles(_localizationFiles);

            _coroutine = diContainer.AddInstance(Coroutines.Create("Project Coroutine", true));

            _ysdk = diContainer.AddInstance(new YandexSDK(_coroutine, _leaderboardName));
            yield return _ysdk.Init_Cn();

            diContainer.AddInstance(_settings);
            diContainer.AddInstance(_settingsColorScriptable.Colors);
            //Banners.InstanceF.Initialize();

            yield return CreateStorage_Cn(diContainer);
            yield return YandexIsLogOn_Cn(diContainer, loadingScreen);

            diContainer.AddInstance(new GameSettings(diContainer));

            Message.Log("End Init Project");
            //----------------------------------

            _startScene.End();

            _settingsColorScriptable.Dispose();
            Destroy(this);

            #region Local: CreateStorage_Cn(..), YandexIsLogOn_Cn(..)
            //=================================
            IEnumerator CreateStorage_Cn(DIContainer container)
            {
                yield return StartCoroutine(CreateStorageService_Cn(container));
                _settings.Init(_ysdk, _projectStorage); ;
            }
            //=================================
            IEnumerator YandexIsLogOn_Cn(DIContainer container, LoadingScreen loadingScreen)
            {
                if (!_ysdk.IsLogOn)
                {
                    loadingScreen.SmoothOff_Wait();
                    yield return _logOnPanel.TryLogOn_Cn(_ysdk, _projectStorage);
                    yield return loadingScreen.SmoothOn_Wait();
                    if (_ysdk.IsLogOn)
                        yield return CreateStorage_Cn(container);
                }
            }
            #endregion
        }

        private IEnumerator CreateStorageService_Cn(DIContainer container)
        {
            if (Create(out IStorageService storage))
            {
                bool result = false;
                yield return storage.Load_Cn((b) => result = b);
                Message.Log(result ? "���������� ���������" : "���������� �� �������");
            }
            else
            {
                Message.Log("StorageService �� ��������");
            }

            container.ReplaceInstance(storage);
            container.ReplaceInstance(_projectStorage = new(storage));

            #region Local: Create(..), Creator()
            // =====================
            bool Create(out IStorageService storage)
            {
                IEnumerator<IStorageService> creator = Creator();
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

                yield return new JsonToYandex(_projectStorageKey, monoBehaviour, _ysdk);
                yield return new JsonToLocalStorage(_projectStorageKey, monoBehaviour);
                yield return new JsonToCookies(_projectStorageKey, monoBehaviour);
            }
            #endregion
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_logOnPanel == null)
                _logOnPanel = FindAnyObjectByType<LogOnPanel>(FindObjectsInactive.Include);
            if (_settingsColorScriptable == null)
                _settingsColorScriptable = EUtility.FindAnyScriptable<TextColorSettingsScriptable>();
            
            _settings.OnValidate();
        }
#endif
    }
}
