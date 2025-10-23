namespace Vurbiri.Colonization
{
    sealed public class ChanceAbility<TId> : Ability<TId> where TId : AbilityId<TId>
    {
        private readonly Ability _ratioA, _ratioB;
        private Chance _chance;

        public ChanceAbility(AAbility<TId> other, Ability ratioA, Ability ratioB) : base(other.Id, other.Value)
        {
            _ratioA = ratioA;
            _ratioB = ratioB;
        }

        public bool Next()
        {
            _chance.Value = _value * (_ratioA.Value + _ratioB.Value);
            return _chance.Roll;
        }
    }
}
