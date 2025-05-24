namespace Vurbiri.Colonization
{
    [System.Serializable]
	public class DiplomacySettings
	{
        public int min = -100;
        public int max = 101;
        public int defaultValue = 25;
        public int penaltyPerRound = -1;
        public int rewardForBuff = 10;
        public int rewardForPresent = 5;
        public int penaltyForFireOnEnemy = -5;
        public int penaltyForFriendlyFire = -10;
    }
}
