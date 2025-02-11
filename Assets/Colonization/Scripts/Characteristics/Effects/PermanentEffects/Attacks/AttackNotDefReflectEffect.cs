//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Attacks\AttackNotDefReflectEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class AttackNotDefReflectEffect : AttackNotDefEffect
    {
        public AttackNotDefReflectEffect(int value) : base(value) { }

        public override int Apply(Actor self, Actor target)
        {
            _value = -base.Apply(self, target);
            return self.ApplyEffect(this);
        }
    }
}
