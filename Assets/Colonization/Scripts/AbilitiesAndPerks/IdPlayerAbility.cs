namespace Vurbiri.Colonization
{
    public class IdTargetObjectPerk : AIdType<IdTargetObjectPerk>
    {
        public const int Player = 0;

        static IdTargetObjectPerk() { RunConstructor(); }
        private IdTargetObjectPerk() { }
    }

    public class IdPlayerAbility : AIdType<IdPlayerAbility>
    {
        public const int None = 0;
        public const int MaxPort = 1;
        public const int MaxUrban = 2;
        public const int MaxShrine = 3;
        public const int MaxRoads = 4;
        public const int IsWall = 5;
        public const int MaxResources = 6;
        public const int ExchangeRate = 7;
        public const int IsFreeGroundRes = 8;
        public const int CompensationRes = 9;
        public const int PortsRatioRes = 10;
        public const int ShrineMaxRes = 11;
        public const int ShrineProfit = 12;
        public const int ShrinePassiveProfit = 13;
        public const int MaxWarrior = 14;

        static IdPlayerAbility() { RunConstructor(); }
        private IdPlayerAbility() { }
    }
}
