using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class BalanceSettings
	{
        public int min = -666;
        public int max = 666;
        public int defaultValue = 0;
        public int penaltyPerDemon = 3;
        public int penaltyPerBlood = 1;
        public int rewardPerShrine = 15;
        public IdArray<WarriorId, int> killWarrior;
        public IdArray<DemonId, int> killDemon;
    }
}
