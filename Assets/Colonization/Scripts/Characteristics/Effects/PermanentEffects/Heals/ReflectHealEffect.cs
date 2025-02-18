//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Heals\ReflectHealEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReflectHealEffect : TargetHealEffect
    {
        private readonly AbilityModifierPercent _reflectMod;

        public ReflectHealEffect(int value, int reflectValue) : base(value)  => _reflectMod = new(-reflectValue);

        public override int Apply(Actor self, Actor target)
        {
            _value = _reflectMod.Apply(base.Apply(self, target));
            return self.ApplyEffect(this);
        }
    }
}
