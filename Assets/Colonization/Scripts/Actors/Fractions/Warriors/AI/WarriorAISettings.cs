using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class WarriorAISettings
	{
		public int maxDistanceUnsiege;
        public int maxDistanceHelp;
        public int maxDistanceEmpty;
        [Space]
        public ReadOnlyIdArray<WarriorId, bool> supports;
        [Space]
        public ReadOnlyIdArray<WarriorId, bool> raiders;
    }
}
