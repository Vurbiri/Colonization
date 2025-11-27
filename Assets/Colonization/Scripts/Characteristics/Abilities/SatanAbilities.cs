using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
	public class SatanAbilities
	{
        [Range(1, 6)] public int gateDefense; // = 4;
        [Range(1, 6)] public int potentialFromLvlRatio; // = 3;
        [Header("-=Curse=-")]
        public int maxCurseBase; //  = 1000;
        [Range(10, 30)] public int maxCursePerLevel; //  = 20;
        [Range(100, 200)] public int cursePerTurn; // = 140;
        [Range(-10, -1)] public int cursePerShrine; // = -5;
        public ReadOnlyIdArray<WarriorId, int> cursePerKillWarrior;
        [Header("-=Spawner=-")]
        [Range(1, 6)] public int minPotentialRatio;
    }
}
