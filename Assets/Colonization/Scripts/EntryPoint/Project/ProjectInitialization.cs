//Assets\Colonization\Scripts\EntryPoint\Project\ProjectInitialization.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectInitialization : MonoBehaviour
    {
        [SerializeField] private SceneId _startScene;
        [Space]
        [SerializeField] private LogOnPanel _logOnPanel;
        [Space]
        [SerializeField] private EnumFlags<Files> _localizationFiles = 0;
        [Space]
        [SerializeField] private string _leaderboardName = "lbColonization";
        [Space]
        [SerializeField] private ColorSettingsScriptable _settingsColorScriptable;
        [SerializeField] private PlayerVisualSetScriptable _playerVisualSetScriptable;
        [Space]
        [SerializeField] private Settings _settings;

        private Coroutines _coroutine;

        public void Init(DIContainer diContainer, Loading loading, ILoadingScreen loadingScreen)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(_startScene);
            operation.allowSceneActivation = false;

            Message.Log("Start Init Project");

            FillingContainer();

            //Banners.InstanceF.Initialize();

            loading.Add(new CreateYandexSDK(diContainer, _coroutine, _leaderboardName));
            loading.Add(new CreateStorage(diContainer, _coroutine, loadingScreen, _logOnPanel));
            loading.Add(new LoadDataStep(diContainer, _playerVisualSetScriptable));
            loading.Add(new EndLoadScene(operation));

            Destroy(this);

            #region Local: FillingContainer()
            //=================================
            void FillingContainer()
            {
                diContainer.AddInstance(Localization.Instance).SetFiles(_localizationFiles);
                diContainer.AddInstance(_coroutine = Coroutines.Create("Project Coroutine", true));
                diContainer.AddInstance(_settings);
                diContainer.AddInstance(_settingsColorScriptable.Colors);

                _settingsColorScriptable.Dispose();
            }
            #endregion
        }

        

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _logOnPanel);
            EUtility.SetScriptable(ref _settingsColorScriptable);
            EUtility.SetScriptable(ref _playerVisualSetScriptable);

            _settings.OnValidate();
        }
#endif
    }
}
