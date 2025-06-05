namespace Vurbiri.Colonization.Characteristics
{
    public abstract class EconomicPerksId : APerkId<EconomicPerksId>
    {
        public const int MaxBlood_1             =  0;
        public const int MaxBlood_2             =  1;
        public const int MaxBlood_3             =  2;
        public const int ShrineProfit_1         =  3;
        public const int ShrinePassiveProfit_1  =  4;

        public const int MaxMainResources_1     =  5;
        public const int MaxMainResources_2     =  6;
        public const int MaxMainResources_3     =  7;
        public const int IsFreeGroundRes_1      =  8;
        public const int CompensationRes_1      =  9;

        public const int IsWall_1               = 10;
        public const int MaxColony_1            = 11;
        public const int IsCity_1               = 12;
        public const int WallDefence_1          = 13;

        public const int IsLighthouse_1         = 14;
        public const int PortsProfit_1          = 15;
        public const int MaxPort_1              = 16;

        public const int MaxRoad_1              = 17;
        public const int MaxRoad_2              = 18;
        public const int MaxRoad_3              = 19;
        public const int MaxRoad_4              = 20;
        public const int MaxRoad_5              = 21;

        public const int ExchangeSaleChance_1   = 22;
        public const int ExchangeSaleChance_2   = 23;
        public const int ExchangeSaleChance_3   = 24;
        public const int ExchangeRate_1         = 25;

        static EconomicPerksId() => ConstructorRun();
    }
}
