namespace Vurbiri.Colonization
{
	public abstract class EconomicSpellId : ASpellId<EconomicSpellId>
    {
        public const int Order         = 0;
        public const int RandomHealing = 1;
        public const int Blessing      = 2;
        public const int Wrath         = 3;
        public const int SummonWarlock = 4;
        public const int Transmutation = 5;
        public const int Sacrifice     = 6;

        [NotId] public const int Type = AbilityTypeId.Economic;

        static EconomicSpellId() => ConstructorRun();
    }
}
