namespace Vurbiri.Colonization
{
    public class PlayerStateId : AStateId<PlayerStateId>
    {
        public const int None                   = -1;
        public const int MaxPort                = 0;
        public const int MaxUrban               = 1;
        public const int MaxShrine              = 2;
        public const int MaxRoads               = 3;
        public const int IsWall                 = 4;
        public const int MaxResources           = 5;
        public const int ExchangeRate           = 6;
        public const int IsFreeGroundRes        = 7;
        public const int CompensationRes        = 8;
        public const int PortsRatioRes          = 9;
        public const int ShrineMaxRes           = 10;
        public const int ShrineProfit           = 11;
        public const int ShrinePassiveProfit    = 12;
        public const int MaxWarrior             = 13;

        static PlayerStateId() => RunConstructor();
        private PlayerStateId() { }
    }
}
