//Assets\Colonization\Scripts\EntryPoint\Project\ProjectInitializationData.cs
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    public class ProjectInitializationData : MonoBehaviour, System.IDisposable
    {
        public LoadScene startScene;
        [Space]
        public LogOnPanel logOnPanel;
        [Space]
        public string leaderboardName = "lbColonization";
        [Space]
        public SettingsTextColorScriptable settingsColorScriptable;
        [Space]
        public SettingsData.Profile defaultProfile;

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
                settingsColorScriptable = VurbiriEditor.Utility.FindAnyScriptable<SettingsTextColorScriptable>();
        }
#endif
    }
}
