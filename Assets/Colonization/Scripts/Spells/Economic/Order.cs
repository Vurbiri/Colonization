namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        private class Order : ASpell
        {
            public Order(SpellBook book) : base(book) { }

            public override bool Init(int playerID) => true;

            public override void Cast(SpellParameters param)
            {
                _book._humans[param.playerId].BuyOrder(param.iValueA * _book._settings.orderPerMana, param.iValueA);
            }
        }
    }
}
