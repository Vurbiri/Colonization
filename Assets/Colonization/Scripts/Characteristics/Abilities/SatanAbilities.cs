using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
	public class SatanAbilities
	{
        public int gateDefense; // = 4;
        public int potentialFromLvlRatio; // = 3;
        [UnityEngine.Header("Curse")]
        public int maxCurseBase; //  = 1000;
        public int maxCursePerLevel; //  = 20;
        public int cursePerTurn; // = 140;
        public int cursePerShrine; // = -5;
        public ReadOnlyIdArray<WarriorId, int> cursePerKillWarrior;
    }
}
