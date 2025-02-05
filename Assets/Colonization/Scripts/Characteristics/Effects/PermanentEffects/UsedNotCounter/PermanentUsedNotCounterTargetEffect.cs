//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\UsedNotCounter\PermanentUsedNotCounterTargetEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class PermanentUsedNotCounterTargetEffect : APermanentUsedNotCounterEffect
    {
        public PermanentUsedNotCounterTargetEffect(int targetAbility, bool isNegative, int usedAbility, Id<TypeModifierId> typeModifier, int value) :
                                  base(targetAbility, isNegative, usedAbility, typeModifier, value)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            Init(self.Abilities);
            target.ApplyEffect(this);
        }

    }
}
