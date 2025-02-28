//Assets\Colonization\Scripts\Characteristics\Abilities\Ids\PlayerAbilityId.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class PlayerAbilityId : AbilityId<PlayerAbilityId>
    {
        public const int MaxPort                = 0;
        public const int MaxUrban               = 1;
        public const int MaxShrine              = 2;
        public const int MaxRoads               = 3;
        public const int IsWall                 = 4;
        public const int MaxMainResources       = 5;
        public const int ExchangeRateMin        = 6;
        public const int ExchangeRateMax        = 7;
        public const int ExchangeMinChance      = 8;
        public const int IsFreeGroundRes        = 9;
        public const int CompensationRes        = 10;
        public const int PortsProfit            = 11;
        public const int MaxBlood               = 12;
        public const int ShrineProfit           = 13;
        public const int ShrinePassiveProfit    = 14;
        public const int MaxWarrior             = 15;
        public const int IsMilitia              = 16;
        public const int IsSolder               = 17;
        public const int IsWizard               = 18;
        public const int IsSaboteur             = 19;
        public const int IsKnight               = 20;
        public const int WallDefence            = 21;
        public const int IsLighthouse           = 22;
        public const int IsCapital              = 23;

        static PlayerAbilityId() => RunConstructor();
        private PlayerAbilityId() { }
    }
}
