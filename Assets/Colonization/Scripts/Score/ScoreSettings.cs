using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class ScoreSettings
	{
		public IdArray<WarriorId, int> killWarrior;
        public IdArray<DemonId, int> killDemon;
        public IdArray<EdificeId, int> buildEdifice;
        public int perOrder;

    }
}
