namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class ActorAISettings
	{
		public bool support;
        public bool raider;
        public UsedDefense defense;
        public UsedHeal heal;
        public UsedSelfSkills selfBuffs;

        public void Init()
        {

        }
    }
}
