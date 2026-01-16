using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
	public class SatanAbilities
	{
        [Range(1, 6)] public int gateDefense; // = 4;
        [Range(1, 4)] public int levelRatio; // = 3;
        [Space]
        public Curse curse;
        [Space]
        public Potential potential;

        #region Nested : Curse, Potential
        //***********************************************
        [System.Serializable]
        public struct Curse
        {
            [Range(800, 2000)] public int maxBase; //  = 1000;
            [Range(-30, 30)] public int perLevel; //  = 20;
            [Space]
            [Range(100, 200)] public int perTurn; // = 140;
            [Range(-10, -1)] public int perShrine; // = -5;
            [Range(1, 20)] public int perRes; // = 10;
            public ReadOnlyIdArray<WarriorId, int> perKillWarrior;
        }
        //***********************************************
        [System.Serializable]
        public struct Potential
        {
            [Range(0, 1)] public int start; // = 1;
            [Range(1, 6)] public int levelRatio; // = 3;
            [Range(1, 6)] public int minRatio;
        }
        //***********************************************
        #endregion
    }
}
