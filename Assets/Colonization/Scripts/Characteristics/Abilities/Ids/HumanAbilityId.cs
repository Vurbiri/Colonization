namespace Vurbiri.Colonization.Characteristics
{
    public class HumanAbilityId : AbilityId<HumanAbilityId>
    {
        public const int MaxShrine              =  0;
        public const int MaxColony              =  1;
        public const int MaxPort                =  2;
        public const int MaxRoad                =  3;
        public const int IsWall                 =  4;
        public const int MaxMainResources       =  5;
        public const int ExchangeRate           =  6;
        public const int ExchangeSaleChance     =  7;
        public const int IsFreeGroundRes        =  8;
        public const int CompensationRes        =  9;
        public const int PortsProfit            = 10;
        public const int MaxBlood               = 11;
        public const int ShrineProfit           = 12;
        public const int ShrinePassiveProfit    = 13;
        public const int MaxWarrior             = 14;
        public const int IsMilitia              = 15;
        public const int IsSolder               = 16;
        public const int IsWizard               = 17;
        public const int IsWarlock              = 18;
        public const int IsKnight               = 19;
        public const int WallDefence            = 20;
        public const int IsLighthouse           = 21;
        public const int IsCity                 = 22;

        static HumanAbilityId() => ConstructorRun();
        private HumanAbilityId() { }
    }
}
