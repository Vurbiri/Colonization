namespace Vurbiri.Colonization
{
	public class MilitarySpellId : ASpellId<MilitarySpellId>
    {
        public const int BloodTrade = 0;
        public const int HealRandom = 1;
        public const int WallBuild = 2;
        public const int Marauding = 3;
        public const int RoadDemolition = 4;

        static MilitarySpellId() => ConstructorRun();
    }
}
