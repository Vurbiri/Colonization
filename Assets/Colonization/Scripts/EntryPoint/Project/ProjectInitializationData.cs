using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class ProjectInitializationData : MonoBehaviour
    {
        public string keySaveProject = "CLN";
        [Space]
        public LoadScene startScene;
        public LogOnPanel logOnPanel;
        [Space]
        public string leaderboardName = "lbColonization";
        [Space]
        public GameplayDefaultData gameplayDefaultData;

        #region Nested: GameplayDefaultData
        //***********************************
        [Serializable]
        public class GameplayDefaultData
        {
            public string keySave = "gsd";
            [Range(0, 100)] public int chanceWater = 33;
        }
        //***********************************
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(logOnPanel == null)
                logOnPanel = FindAnyObjectByType<LogOnPanel>();
        }
#endif
    }
}
