using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class ScientistSettings
    {
        public ReadOnlyIdArray<AbilityTypeId, Id<PlayerId>> specialization;
        public ReadOnlyIdArray<AbilityTypeId, ReadOnlyArray<int>> weights;
        public int shift;
    }
}
