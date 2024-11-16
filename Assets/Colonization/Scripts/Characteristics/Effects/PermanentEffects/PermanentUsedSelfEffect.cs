namespace Vurbiri.Colonization.Characteristics
{
    using Actors;

    public class PermanentUsedSelfEffect : APermanentUsedEffect
    {
        public PermanentUsedSelfEffect(int targetAbility, bool isNegative, int usedAbility, int counteredAbility, Id<TypeModifierId> typeModifier, int value) : 
                                  base(targetAbility, isNegative, usedAbility, counteredAbility, typeModifier, value)
        {
        }

        public override void Apply(Actor self, Actor target) => self.ApplyEffect(this);
    }
}
