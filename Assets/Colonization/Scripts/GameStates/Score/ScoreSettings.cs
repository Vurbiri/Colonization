using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class ScoreSettings
	{
		public IdArray<WarriorId, int> killWarrior;
        public IdArray<DemonId, int> killDemon;
        [Header("-= Building =-")]
        public int perRoad;
        public int perWall;
        public IdArray<EdificeId, int> buildEdifice;
        [Header("-= Order =-")]
        public int perOrder;

    }
}
