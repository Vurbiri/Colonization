//Assets\Colonization\Scripts\EntryPoint\Project\ProjectInitialization.cs
using System.Collections;
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

        private YandexSDK _ysdk;
        private ProjectStorage _projectStorage;

        public IEnumerator Init_Cn(DIContainer diContainer, LoadingScreen loadingScreen)
        {
            _startScene.Start();

            //----------------------------------
            Message.Log("Start Init Project");

            diContainer.AddInstance(Localization.Instance).SetFiles(_localizationFiles);

            var coroutine = diContainer.AddInstance(Coroutines.Create("Project Coroutine", true));

            _ysdk = diContainer.AddInstance(new YandexSDK(coroutine, _leaderboardName));
            yield return _ysdk.Init_Cn();

            diContainer.AddInstance(_settings);
            diContainer.AddInstance(_settingsColorScriptable.Colors);
            //Banners.InstanceF.Initialize();

            yield return CreateStorage_Cn();
            yield return YandexIsLogOn_Cn(loadingScreen);

            diContainer.AddInstance(new GameSettings(diContainer));

            Message.Log("End Init Project");
            //----------------------------------

            _startScene.End();

            _settingsColorScriptable.Dispose();
            Destroy(this);

            #region Local: CreateStorage_Cn(..), YandexIsLogOn_Cn(..)
            //=================================
            IEnumerator CreateStorage_Cn()
            {
                yield return StartCoroutine(Vurbiri.Storage.Create_Cn(diContainer, _projectStorageKey, storage =>
                {
                    _projectStorage = diContainer.ReplaceInstance(new ProjectStorage(storage));
                    _settings.Init(_ysdk, _projectStorage);
                }));
            }
            //=================================
            IEnumerator YandexIsLogOn_Cn(LoadingScreen loadingScreen)
            {
                if (!_ysdk.IsLogOn)
                {
                    loadingScreen.SmoothOff_Wait();
                    yield return _logOnPanel.TryLogOn_Cn(_ysdk, _projectStorage);
                    yield return loadingScreen.SmoothOn_Wait();
                    if (_ysdk.IsLogOn) yield return CreateStorage_Cn();
                }
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
