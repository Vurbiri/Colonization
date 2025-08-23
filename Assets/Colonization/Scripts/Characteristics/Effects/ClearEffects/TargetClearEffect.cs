using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class TargetClearEffect : AClearEffect
    {
        public TargetClearEffect(Id<ClearEffectsId> type, int value) : base(type, value)
        {
        }

        public override int Apply(Actor self, Actor target)
        {
            target.ClearEffects(_value, _type);
            return 0;
        }
    }
}
