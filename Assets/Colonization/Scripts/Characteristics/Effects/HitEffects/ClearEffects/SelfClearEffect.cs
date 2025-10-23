namespace Vurbiri.Colonization.Characteristics
{
    sealed public class SelfClearEffect : AClearEffect
    {
        public SelfClearEffect(Id<ClearEffectsId> type, int value) : base(type, value)
        {
        }

        public override int Apply(Actor self, Actor target)
        {
            self.ClearEffects(_value, _type);
            return 0;
        }
    }
}
