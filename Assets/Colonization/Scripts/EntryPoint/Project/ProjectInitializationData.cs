using UnityEngine;

namespace Vurbiri.Colonization
{
    public class ProjectInitializationData : MonoBehaviour
    {
        public LoadScene startScene;
        [Space]
        public LogOnPanel logOnPanel;
        [Space]
        public string leaderboardName = "lbColonization";
        [Space]
        public SettingsData.Profile defaultProfile;
        [Space]
        public GameplaySettingsData.Settings gameplayDefaultData;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(logOnPanel == null)
                logOnPanel = FindAnyObjectByType<LogOnPanel>();
        }
#endif
    }
}
