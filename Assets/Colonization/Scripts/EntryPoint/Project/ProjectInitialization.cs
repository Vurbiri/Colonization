using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectInitialization : MonoBehaviour
    {
        [SerializeField] private SceneId _startScene;
        [Space]
        [SerializeField] private LogOnPanel _logOnPanel;
        [SerializeField] private LoadingScreen _loadingScreen;
        [Space]
        [SerializeField] private FileIds _localizationFiles = new(false);
        [Space]
        [SerializeField] private string _leaderboardName = "lbColonization";
        [Space]
        [SerializeField] private ColorSettingsScriptable _settingsColorScriptable;
        [SerializeField] private PlayerVisualSetScriptable _playerVisualSetScriptable;
        [Space]
        [SerializeField] private Settings _settings;

        public ILoadingScreen Screen => _loadingScreen;

        public void Init(ProjectContent content, Loading loading, MonoBehaviour mono)
        {
             AsyncOperation operation = SceneManager.LoadSceneAsync(_startScene);
            operation.allowSceneActivation = false;

            Log.Info("Start Init Project");

            Localization.Instance.SetFiles(_localizationFiles);

            content.settings = _settings;
            content.projectColors = _settingsColorScriptable.Colors;

            MessageBox.SetColors(content.projectColors.PanelBack, content.projectColors.TextDefault);

            loading.Add(new CreateYandexSDK(content, mono, _leaderboardName));
            loading.Add(new CreateStorage(content, mono, _loadingScreen, _logOnPanel));
            loading.Add(new LoadSettingsStep(content, _playerVisualSetScriptable));
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
