namespace Vurbiri.Colonization
{
    sealed public class ChanceAbility<TId> : Ability<TId> where TId : AbilityId<TId>
    {
        private readonly Ability _ratio;
        private Chance _chance;

        public ChanceAbility(AAbility<TId> other, Ability ratio) : base(other.Id, other.Value)
        {
            _ratio = ratio;
        }

        public bool Next()
        {
            _chance.Value = _value * _ratio.Value / 100;
            return _chance.Roll;
        }
    }
}
