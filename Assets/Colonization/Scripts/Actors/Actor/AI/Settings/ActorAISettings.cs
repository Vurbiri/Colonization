namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class ActorAISettings
	{
		public bool support;
        public bool raider;
        public Chance specChance;
        public UsedSelfSkill defenseSkill;
        public UsedSelfSkill selfBuffInCombat;
        public UsedHeal heal;
    }
}
