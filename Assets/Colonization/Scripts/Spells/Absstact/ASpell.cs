namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        private abstract class ASpell
        {
            public abstract void Cast(SpellParam param, CurrenciesLite resources);
        }
    }
}
