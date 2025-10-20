namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Marauding : Cast
            {
                public Marauding(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.Marauding) { }

                public override System.Collections.IEnumerator TryCasting_Cn() => Casting_Cn();
            }
        }
    }
}
