namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        private abstract class ASpell
        {
            public bool canUse = true;

            public virtual void Update() {}

            public abstract bool Cast(SpellParam param);

        }
    }
}
