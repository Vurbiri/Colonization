using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class HolyAttackEffect : AttackEffect
    {
        public HolyAttackEffect(int value, int pierce) : base(value, pierce)
        {
        }

        public override int Apply(Actor self, Actor target)
        {
           return target.TypeId == ActorTypeId.Demon ? base.Apply(self, target) : 0;
        }
    }
}
