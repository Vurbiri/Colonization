namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Marauding : Cast
            {
                private Marauding(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.Marauding) { }
                public static void Create(Caster parent) => new Marauding(parent);

                public override System.Collections.IEnumerator TryCasting_Cn() => Casting_Cn();
            }
        }
    }
}
