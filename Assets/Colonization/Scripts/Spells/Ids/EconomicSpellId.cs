namespace Vurbiri.Colonization
{
	public abstract class EconomicSpellId : ASpellId<EconomicSpellId>
    {
        public const int Order = 0;
        public const int Blessing = 1;
        public const int Wrath = 2;
        public const int SummonWarlock = 3;
        public const int ShuffleRes = 4;

        static EconomicSpellId() => ConstructorRun();
    }
}
