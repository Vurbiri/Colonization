using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class ScientistSettings
    {
        public Id<PlayerId> economist;
        public Id<PlayerId> militarist;
        public int shiftMain;
        public ReadOnlyIdArray<EconomicPerksId, int> economic;
        public ReadOnlyIdArray<MilitaryPerksId, int> military;
    }
}
