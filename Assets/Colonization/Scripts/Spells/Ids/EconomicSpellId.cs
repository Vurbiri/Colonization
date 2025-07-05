namespace Vurbiri.Colonization
{
	public abstract class EconomicSpellId : ASpellId<EconomicSpellId>
    {
        public const int Order = 0;

        static EconomicSpellId() => ConstructorRun();
    }
}
