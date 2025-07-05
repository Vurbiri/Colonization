namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Order : APlayerSpell
        {
            public Order(int playerId) : base(playerId) { }

            public override bool Cast(SpellParam param)
            {
                s_humans[_playerId].BuyOrder(param.iValueA * s_settings.orderPerMana, param.iValueA);
                return true;
            }
        }
    }
}
