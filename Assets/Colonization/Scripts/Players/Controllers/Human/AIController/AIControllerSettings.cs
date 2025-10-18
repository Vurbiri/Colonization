namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class AIControllerSettings
	{
        public WaitRealtime waitPlay;
        [UnityEngine.Space]
        public Id<PlayerId> militarist;
        public int minExchangeBlood; // = 4;

    }
}
