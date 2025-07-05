namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        private abstract class ASpell
        {
           
            public abstract bool Cast(SpellParam param, CurrenciesLite resources);

        }
    }
}
