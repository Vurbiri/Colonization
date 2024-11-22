//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\PermanentUsedReflectEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class PermanentUsedReflectEffect : APermanentUsedEffect
    {
        public PermanentUsedReflectEffect(int targetAbility, bool isNegative, int usedAbility, int counteredAbility, Id<TypeModifierId> typeModifier, int value) :
                                  base(targetAbility, isNegative, usedAbility, counteredAbility, typeModifier, value)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            Init(self.Abilities, target.Abilities);
            target.ApplyEffect(this);
        }
    }
}
