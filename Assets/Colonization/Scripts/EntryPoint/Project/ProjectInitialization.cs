//Assets\Colonization\Scripts\EntryPoint\Project\ProjectInitialization.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Data;
using Vurbiri.Colonization.UI;
using Vurbiri.TextLocalization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectInitialization : MonoBehaviour
    {
        [SerializeField] private LoadScene _startScene;
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

        public IEnumerator Init_Cn(DIContainer servicesContainer, DIContainer dataContainer, LoadingScreen loadingScreen)
        {
            _startScene.Start();

            //----------------------------------
            Message.Log("Start Init Project");

            servicesContainer.AddInstance(Localization.Instance).SetFiles(_localizationFiles);

            var coroutine = servicesContainer.AddInstance(Coroutines.Create("Project Coroutine", true));

            var ysdk = servicesContainer.AddInstance(new YandexSDK(coroutine, _leaderboardName));
            yield return ysdk.Init_Cn();

            dataContainer.AddInstance(_settings);
            dataContainer.AddInstance(_settingsColorScriptable.Colors);
            //Banners.InstanceF.Initialize();

            yield return CreateStorage_Cn(ysdk);
            yield return YandexIsLogOn_Cn(ysdk, loadingScreen);

            dataContainer.AddInstance(new GameSettings(servicesContainer));

            Message.Log("End Init Project");
            //----------------------------------

            _startScene.End();

            _settingsColorScriptable.Dispose();
            Destroy(this);

            #region Local: CreateStorage_Cn(..), YandexIsLogOn_Cn(..)
            //=================================
            IEnumerator CreateStorage_Cn(YandexSDK ysdk)
            {
                yield return StartCoroutine(Storage.Create_Cn(servicesContainer, SAVE_KEYS.PROJECT, storage =>
                {
                    var projectSaveData = dataContainer.ReplaceInstance(new ProjectSaveData(storage));
                    _settings.Init(ysdk, projectSaveData);
                }));
            }
            //=================================
            IEnumerator YandexIsLogOn_Cn(YandexSDK ysdk, LoadingScreen loadingScreen)
            {
                if (!ysdk.IsLogOn)
                {
                    loadingScreen.SmoothOff_Wait();
                    yield return _logOnPanel.TryLogOn_Cn(ysdk);
                    yield return loadingScreen.SmoothOn_Wait();
                    if (ysdk.IsLogOn) yield return CreateStorage_Cn(ysdk);
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
