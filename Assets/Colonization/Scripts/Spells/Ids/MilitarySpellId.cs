namespace Vurbiri.Colonization
{
	public class MilitarySpellId : ASpellId<MilitarySpellId>
    {
        public const int BloodTrade     = 0;
        public const int Spying         = 1;
        public const int WallBuild      = 2;
        public const int Marauding      = 3;
        public const int RoadDemolition = 4;
        public const int SwapId         = 5;
        public const int Zeal           = 6;

        [NotId] public const int Type = AbilityTypeId.Military;

        static MilitarySpellId() => ConstructorRun();
    }
}
