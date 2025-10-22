namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class AIControllerSettings
	{
        public WaitRealtime waitPlayStart;
        public WaitRealtime waitPlay;
        [UnityEngine.Space]
        public Id<PlayerId> militarist;
        [UnityEngine.Space]
        public int percentBloodOffset;
        public int minExchangeBlood;
    }
}
