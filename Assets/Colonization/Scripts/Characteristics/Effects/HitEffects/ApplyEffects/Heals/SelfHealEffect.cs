namespace Vurbiri.Colonization
{
    sealed public class SelfHealEffect : AHealEffect
    {
        public SelfHealEffect(int value) : base(value) { }

        public override int Apply(Actor self, Actor target)
        {
            Init(self.Abilities);
            return self.ApplyEffect(this);
        }
    }
}
