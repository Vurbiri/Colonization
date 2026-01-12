using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
	public class SatanAbilities
	{
        [Range(1, 6)] public int gateDefense; // = 4;
        [Header("-=Curse=-")]
        [Range(800, 2000)] public int maxCurseBase; //  = 1000;
        [Range(-30, 30)] public int maxCursePerLevel; //  = 20;
        [Space]
        [Range(100, 200)] public int cursePerTurn; // = 140;
        [Range(-10, -1)] public int cursePerShrine; // = -5;
        [Range(1, 20)] public int cursePerRes; // = 10;
        public ReadOnlyIdArray<WarriorId, int> cursePerKillWarrior;
        [Header("-=Spawner=-")]
        [Range(1, 6)] public int potentialFromLvlRatio; // = 3;
        [Range(1, 6)] public int minPotentialRatio;
    }
}
