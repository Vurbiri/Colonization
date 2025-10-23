namespace Vurbiri.Colonization
{
    public class TargetHealEffect : AHealEffect
    {
        public TargetHealEffect(int value) : base(value) { }

        public override int Apply(Actor self, Actor target)
        {
            Init(self.Abilities);
            return target.ApplyEffect(this);
        }
    }
}
