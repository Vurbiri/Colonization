namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        private abstract class APlayerSpell : ASpell
        {
            protected readonly int _playerId;

            public APlayerSpell(int playerId)
            {
                _playerId = playerId;
            }
        }
    }
}
