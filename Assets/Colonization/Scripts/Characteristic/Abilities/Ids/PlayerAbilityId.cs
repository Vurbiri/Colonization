namespace Vurbiri.Colonization
{
    public class PlayerAbilityId : AAbilityId<PlayerAbilityId>
    {
        public const int MaxPort                = 0;
        public const int MaxUrban               = 1;
        public const int MaxShrine              = 2;
        public const int MaxRoads               = 3;
        public const int IsWall                 = 4;
        public const int MaxMainResources       = 5;
        public const int ExchangeRate           = 6;
        public const int IsFreeGroundRes        = 7;
        public const int CompensationRes        = 8;
        public const int PortsAddRes            = 9;
        public const int MaxBlood               = 10;
        public const int ShrineProfit           = 11;
        public const int ShrinePassiveProfit    = 12;
        public const int MaxWarrior             = 13;
        public const int IsMilitia              = 14;
        public const int IsSolder               = 15;
        public const int IsWizard               = 16;
        public const int IsSaboteur             = 17;
        public const int IsKnight               = 18;
        public const int WallDefence            = 19;
        public const int IsLighthouse           = 20;
        public const int IsCapital              = 21;

        static PlayerAbilityId() => RunConstructor();
        private PlayerAbilityId() { }
    }
}
