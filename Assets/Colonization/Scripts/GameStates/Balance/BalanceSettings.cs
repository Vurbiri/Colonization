using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class BalanceSettings
	{
        public int min = -666;
        public int max = 666;
        [Header("-=Default=-")]
        public int defaultValue = 0;
        [Header("-=Penalty=-")]
        public int penaltyPerDemon = -3;
        public int penaltyPerSatanLevelUp = -2;
        public int penaltyPerBlood = -1;
        public IdArray<WarriorId, int> penaltyPerKillWarrior;
        [Header("-=Reward=-")]
        public int rewardPerShrine = 15;
        public IdArray<DemonId, int> rewardPerKillDemon;
    }
}
