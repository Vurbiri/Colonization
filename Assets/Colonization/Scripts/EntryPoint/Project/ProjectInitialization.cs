//Assets\Colonization\Scripts\EntryPoint\Project\ProjectInitialization.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectInitialization : MonoBehaviour
    {
        [SerializeField] private SceneId _startScene;
        [Space]
        [SerializeField] private LogOnPanel _logOnPanel;
        [SerializeField] private LoadingScreen _loadingScreen;
        [Space]
        [SerializeField] private EnumFlags<Files> _localizationFiles = 0;
        [Space]
        [SerializeField] private string _leaderboardName = "lbColonization";
        [Space]
        [SerializeField] private ColorSettingsScriptable _settingsColorScriptable;
        [SerializeField] private PlayerVisualSetScriptable _playerVisualSetScriptable;
        [Space]
        [SerializeField] private Settings _settings;

        public ILoadingScreen Screen => _loadingScreen;

        public void Init(DIContainer diContainer, Loading loading)
        {
            Coroutines coroutine;
            AsyncOperation operation = SceneManager.LoadSceneAsync(_startScene);
            operation.allowSceneActivation = false;

            Message.Log("Start Init Project");

            diContainer.AddInstance(Localization.Instance).SetFiles(_localizationFiles);
            diContainer.AddInstance(coroutine = Coroutines.Create("Project Coroutine", true));
            diContainer.AddInstance(_settings);
            diContainer.AddInstance(_settingsColorScriptable.Colors);

            //Banners.InstanceF.Initialize();

            loading.Add(new CreateYandexSDK(diContainer, coroutine, _leaderboardName));
            loading.Add(new CreateStorage(diContainer, coroutine, _loadingScreen, _logOnPanel));
            loading.Add(new LoadDataStep(diContainer, _playerVisualSetScriptable));
            loading.Add(new EndLoadScene(operation));

            Destroy(this);
            _settingsColorScriptable.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _logOnPanel);
            EUtility.SetObject(ref _loadingScreen);
            EUtility.SetScriptable(ref _settingsColorScriptable);
            EUtility.SetScriptable(ref _playerVisualSetScriptable);

            _settings.OnValidate();
        }
#endif
    }
}
