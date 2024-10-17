using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    public class ProjectInitializationData : MonoBehaviourDisposable
    {
        public LoadScene startScene;
        [Space]
        public LogOnPanel logOnPanel;
        [Space]
        public string leaderboardName = "lbColonization";
        [Space]
        public SettingsData.Profile defaultProfile;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if(logOnPanel == null)
                logOnPanel = FindAnyObjectByType<LogOnPanel>();
        }
#endif
    }
}
