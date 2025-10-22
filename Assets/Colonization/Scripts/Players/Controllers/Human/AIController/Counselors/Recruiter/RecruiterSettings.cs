using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class RecruiterSettings
	{
        public ReadOnlyIdArray<WarriorId, int> weights;
    }
}
