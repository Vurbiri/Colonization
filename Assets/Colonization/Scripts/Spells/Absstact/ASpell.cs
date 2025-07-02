namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        private abstract class ASpell
        {
            protected readonly SpellBook _book;

            public ASpell(SpellBook book)
            {
                _book = book;
            }

            public abstract bool Init(int playerID);

            public abstract void Cast(SpellParameters param);

        }
    }
}
