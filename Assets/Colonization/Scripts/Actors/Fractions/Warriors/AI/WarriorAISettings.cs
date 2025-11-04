using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class WarriorAISettings
	{
		public int maxDistanceUnsiege;
        public int maxDistanceSupport;
        public int maxDistanceEmpty;
        [Space]
        public ReadOnlyIdArray<WarriorId, bool> supports;
    }
}
