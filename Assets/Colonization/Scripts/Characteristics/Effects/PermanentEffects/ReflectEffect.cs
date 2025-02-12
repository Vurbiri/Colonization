//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\ReflectEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReflectEffect : AEffect
    {
        public ReflectEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value) : base(targetAbility, typeModifier, value) { }

        public override int Apply(Actor self, Actor target)
        {
            int temp = _value;
            _value = -target.ApplyEffect(this);
            self.ApplyEffect(this);
            return _value = temp;
        }
    }
}
