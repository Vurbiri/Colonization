using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class BuilderSettings
	{
        public int profitWeight;
        public int costWeight;
        public ReadOnlyIdArray<EdificeId, int> edificeWeight;
        public int wallWeight;
    }
}
