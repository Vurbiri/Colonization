using UnityEngine;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class AIControllerSettings
	{
        public WaitRealtime waitPlayStart;
        public WaitRealtime waitPlay;
        [Space]
        public Id<PlayerId> militarist;
        [Space]
        public int percentBloodOffset;
        public int minExchangeBlood;
        [Header("Cheat")]
        public int minPercentRes;
        public int addRes;
    }
}
