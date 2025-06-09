namespace Vurbiri.Colonization.Characteristics
{
    public abstract class HumanAbilityId : AbilityId<HumanAbilityId>
    {
        public const int MaxShrine              =  0;
        public const int MaxColony              =  1;
        public const int MaxPort                =  2;
        public const int MaxRoad                =  3;
        public const int MaxMainResources       =  4;
        public const int CompensationRes        =  5;
        public const int IsFreeGroundRes        =  6;
        public const int MaxBlood               =  7;
        public const int ShrineProfit           =  8;
        public const int ShrinePassiveProfit    =  9;
        public const int PortsProfitShift       = 10;
        public const int ExchangeRate           = 11;
        public const int ExchangeSaleChance     = 12;
        public const int MaxWarrior             = 13;
        public const int IsMilitia              = 14;
        public const int IsSolder               = 15;
        public const int IsWizard               = 16;
        public const int IsWarlock              = 17;
        public const int IsKnight               = 18;
        public const int IsWall                 = 19;
        public const int WallDefence            = 20;
        public const int IsLighthouse           = 21;
        public const int IsCity                 = 22;

        static HumanAbilityId() => ConstructorRun();
    }
}
