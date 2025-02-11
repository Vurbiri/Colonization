//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\Used\PermanentUsedReflectEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class AttackReflectEffect : AttackEffect
    {
        public AttackReflectEffect(int value) :  base( value) { }

        public override int Apply(Actor self, Actor target)
        {
            _value = -base.Apply(self, target);
            return self.ApplyEffect(this);
        }
    }
}
