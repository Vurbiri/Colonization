namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        private abstract class APlayerSpell : ASpell
        {
            protected APlayerSpell(SpellBook book, int type, int id)
            {
                book._spells[type][id] = this;
            }
        }
    }
}
