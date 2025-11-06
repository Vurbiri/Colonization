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
        public Chance blockChance;
        public Chance preBuffChance;
        public UsedSkills<WarriorId> preBuff;
        [Space]
        [Range(0, 5)] public int maxDistanceEmpty;
        [Space]
        public ReadOnlyIdArray<WarriorId, bool> supports;
        [Space]
        public ReadOnlyIdArray<WarriorId, bool> raiders;
    }
}
