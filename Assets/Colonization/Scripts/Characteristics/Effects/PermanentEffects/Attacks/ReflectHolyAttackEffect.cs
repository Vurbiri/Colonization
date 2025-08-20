using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class ReflectHolyAttackEffect : ReflectAttackEffect
    {
        public ReflectHolyAttackEffect(int value, int defenseValue, int reflectValue) : base(value, defenseValue, reflectValue)
        {
        }

        public override int Apply(Actor self, Actor target)
        {
            return target.TypeId == ActorTypeId.Demon ? base.Apply(self, target) : 0;
        }
    }
}
