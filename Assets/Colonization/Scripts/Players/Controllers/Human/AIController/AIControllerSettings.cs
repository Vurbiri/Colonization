using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class AIControllerSettings
	{
        public ReadOnlyIdArray<AbilityTypeId, Id<PlayerId>> specialization;
        public int minExchangeBlood; // = 4;

    }
}
