namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Spying : Cast
            {
                public Spying(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.Spying) { }

                public override System.Collections.IEnumerator TryCasting_Cn() => Casting_Cn();
            }
        }
    }
}
