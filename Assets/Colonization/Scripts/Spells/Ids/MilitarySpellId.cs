namespace Vurbiri.Colonization
{
	public class MilitarySpellId : ASpellId<MilitarySpellId>
    {
        public const int BloodTrade = 0;
        public const int HealRandom = 1;

        static MilitarySpellId() => ConstructorRun();
    }
}
