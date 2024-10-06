namespace Vurbiri.Colonization
{
    public class PlayerAbility : AAbility<PlayerAbilityId>
    {
        public PlayerAbility(int id, int baseValue) : base(new(id), baseValue) { }
        public PlayerAbility(Id<PlayerAbilityId> id, int baseValue) : base(id, baseValue) { }

        public PlayerAbility(PlayerAbility ability) : base(ability) { }
    }
}
