namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
	public class SatanAbilities
	{
        public int gateDefense = 4;
        public int maxCurse = 100;
        public int maxCursePerLevel = 2;
        public int curseProfit = 33;
        public int curseProfitPerLevel = 3;
        public int cursePerTurn = 13;

        public int ratioRewardCurse = 50;
        public int ratioPenaltyCurse = 15;

        public const int SHIFT_RATIO = 10;
    }
}
