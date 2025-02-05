//Assets\Colonization\Scripts\Characteristics\Effects\PermanentEffects\UsedNotCounter\PermanentUsedNotCounterSelfEffect.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class PermanentUsedNotCounterSelfEffect : APermanentUsedNotCounterEffect
    {
        public PermanentUsedNotCounterSelfEffect(int targetAbility, bool isNegative, int usedAbility, Id<TypeModifierId> typeModifier, int value) :
                                  base(targetAbility, isNegative, usedAbility, typeModifier, value)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            Init(self.Abilities);
            self.ApplyEffect(this);
        }
    }
}
