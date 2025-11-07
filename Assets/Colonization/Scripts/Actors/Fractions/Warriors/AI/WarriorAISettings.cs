using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class WarriorAISettings
	{
		[Range(0, 5)]   public int maxDistanceUnsiege;
        [Range(0, 100)] public int minHPUnsiege;
        [Space]
        [Range(0, 5)]   public int maxDistanceHelp;
        [Range(0, 100)] public int minHPHelp;
        [Space]
        [Range(0, 5)] public int maxDistanceColony;
        [Range(0, 5)] public int maxDistanceRaid;
        [Space]
        [Range(0, 100)] public int blockChance;
        public UsedSkills<WarriorId> defenseBuff;
        [Space]
        public ReadOnlyIdArray<WarriorId, bool> supports;
        [Space]
        public ReadOnlyIdArray<WarriorId, bool> raiders;
    }
}
