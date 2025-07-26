namespace Vurbiri.Colonization
{
	public abstract class EconomicSpellId : ASpellId<EconomicSpellId>
    {
        public const int Order            = 0;
        public const int Blessing         = 1;
        public const int Wrath            = 2;
        public const int SummonWarlock    = 3;
        public const int ShuffleResources = 4;
        public const int HalvingResources = 5;
        public const int Sacrifice        = 6;

        [NotId] public const int Type = TypeOfPerksId.Economic;

        static EconomicSpellId() => ConstructorRun();
    }
}
