//Assets\Colonization\Scripts\EntryPoint\Project\ProjectInitializationData.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.UI;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.EntryPoint
{
    public class ProjectInitializationData : MonoBehaviour, System.IDisposable
    {
        public LoadScene startScene;
        [Space]
        public LogOnPanel logOnPanel;
        [Space]
        public EnumArray<Files, bool> localizationFiles = new(false);
        [Space]
        public string leaderboardName = "lbColonization";
        [Space]
        public TextColorSettingsScriptable settingsColorScriptable;
        [Space]
        public Settings settings;

        public void Dispose()
        {
            settingsColorScriptable.Dispose();
            Destroy(this);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if(logOnPanel == null)
                logOnPanel = FindAnyObjectByType<LogOnPanel>();
            if (settingsColorScriptable == null)
                settingsColorScriptable = EUtility.FindAnyScriptable<TextColorSettingsScriptable>();
            
            settings.OnValidate();
        }
#endif
    }
}
