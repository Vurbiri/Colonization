namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class ActorAISettings
	{
		public bool support;
        public bool raider;
        public UsedDefense defense;
        public UsedHeal heal;
        public UsedSelfBuffs selfBuffs;
        public UsedDebuffs debuffs;
        public UsedAttacks attacks;
        public UsedBuffs buffs;

        public void Init()
        {
            attacks.Init();
        }
    }
}
