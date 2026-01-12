using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;
using Vurbiri.Web;

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
        [SerializeField] private Prices _prices;
        [SerializeField] private ColorSettingsScriptable _settingsColorScriptable;
        [SerializeField] private PlayerVisualSetScriptable _playerVisualSetScriptable;
        [Space]
        [SerializeField] private Settings _settings;

        public ILoadingScreen Screen => _loadingScreen;

        public void Init(ProjectContent content, Loading loading, MonoBehaviour mono)
        {
            Log.Create<WebLog>();

            AsyncOperation operation = SceneManager.LoadSceneAsync(_startScene);
            operation.allowSceneActivation = false;

            Log.Info("[ProjectInitialization] Start");

            Localization.Instance.SetFiles(_localizationFiles, false);

            content.settings = _settings;
            content.prices = _prices;
            content.projectColors = _settingsColorScriptable;

            SetColors(content.projectColors);

#if YSDK
            loading.Add(new CreateYandexSDK(content, mono, _leaderboardName));
#endif
            loading.Add(new CreateStorage(content, mono, _loadingScreen, _logOnPanel));
            loading.Add(new LoadSettingsStep(content, _playerVisualSetScriptable));
            loading.Add(new EndLoadScene(operation));

            Destroy(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetColors(ProjectColors colors)
        {
            Banner.Colors[MessageTypeId.Info]    = colors.TextDefault;
            Banner.Colors[MessageTypeId.Warning] = colors.TextWarning;
            Banner.Colors[MessageTypeId.Error]   = colors.TextNegative;
            Banner.Colors[MessageTypeId.Profit]  = colors.TextPositive;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _logOnPanel);
            EUtility.SetObject(ref _loadingScreen);
            EUtility.SetScriptable(ref _prices);
            EUtility.SetScriptable(ref _settingsColorScriptable);
            EUtility.SetScriptable(ref _playerVisualSetScriptable);

            _settings.OnValidate();
        }
#endif
    }
}
