namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            sealed private class Spying : Cast
            {
                private Spying(Caster parent) : base(parent, MilitarySpellId.Type, MilitarySpellId.Spying) { }
                public static void Create(Caster parent) => new Spying(parent);

                public override System.Collections.IEnumerator TryCasting_Cn() => Casting_Cn();
            }
        }
    }
}
