//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\PermanentReflectEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class PermanentReflectEffect : AEffect
    {
        public PermanentReflectEffect(int targetAbility, bool isNegative, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier)
        {
            _value = isNegative ? -value : value;
        }

        public override void Apply(Actor self, Actor target)
        {
            int temp = _value;
            _value = -target.ApplyEffect(this);
            self.ApplyEffect(this);
            _value = temp;
        }
    }
}
