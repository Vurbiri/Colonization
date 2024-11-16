namespace Vurbiri.Colonization.Characteristics
{
    using Actors;

    public class PermanentSelfEffect : AEffect
    {
        public PermanentSelfEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier)
        {
            _value = isNegative ? -value : value;
        }

        public override void Apply(Actor self, Actor target) => self.ApplyEffect(this);
    }
}
