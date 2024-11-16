using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class PermanentUsedTargetEffect : APermanentUsedEffect
    {
        public PermanentUsedTargetEffect(int targetAbility, bool isNegative, int usedAbility, int counteredAbility, Id<TypeModifierId> typeModifier, int value) :
                                  base(targetAbility, isNegative, usedAbility, counteredAbility, typeModifier, value)
        {
        }

        public override void Apply(Actor self, Actor target)
        {
            Init(self.Abilities, target.Abilities);
            _value = -target.ApplyEffect(this);
            self.ApplyEffect(this);
        }
    }
}
