namespace Vurbiri.Colonization
{
    sealed public class ChanceAbility<TId> : Ability<TId> where TId : AbilityId<TId>
    {
        private readonly Ability _ratio;
        private Chance _chance;

        public ChanceAbility(AAbility<TId> original, Ability ratio) : base(original.Id, original.Value)
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
