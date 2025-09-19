using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
	public class DiplomacySettings
	{
        public int min; // = -99;
        public int max; // = 100;
        public int great; // = 75;
        [Header("-=Default=-")]
        public int defaultValue; // = 25;
        public int defaultDelta; // = 5;
        [Header("-=Round=-")]
        public IntRnd personPerRound; // = new(-1, 1);
        public IntRnd aiPerRound; // = new(-1, 2);
        [Header("-=Reward=-")]
        public int rewardForBuff; // = 10;
        public int rewardForGift; // = 5;
        [Header("-=Penalty=-")]
        public int penaltyForMarauding; // = -6;
        public int penaltyForFireOnEnemy; // = -3;
        public int penaltyForFriendlyFire; // = -8;
    }
}
