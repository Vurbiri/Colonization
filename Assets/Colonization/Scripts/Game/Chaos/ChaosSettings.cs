using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class ChaosSettings
	{
        public int max; // = 666;
        [Header("-=Penalty=-")]
        public int penaltyPerTurn;
        public int penaltyPerDemon;
        public int penaltyPerBlood; 
        public ReadOnlyIdArray<WarriorId, int> penaltyPerKillWarrior;
        [Header("-=Reward=-")]
        public int rewardPerShrine; 
        public ReadOnlyIdArray<DemonId, int> rewardPerKillDemon;
    }
}
