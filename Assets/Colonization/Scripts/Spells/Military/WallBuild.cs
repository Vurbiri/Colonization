using System.Collections.Generic;
using static Vurbiri.Colonization.TypeOfPerksId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class WallBuild : ASpell
        {
            private readonly List<int> _canWall = new(CONST.DEFAULT_MAX_EDIFICES);
            private readonly CurrenciesLite _cost;
            private WallBuild(Prices prices) 
            {
                _cost = new(prices.Wall)
                {
                    { CurrencyId.Mana, s_costs[Military][MilitarySpellId.WallBuild] }
                };
            }
            public static void Create(Prices prices) => s_spells[Military][MilitarySpellId.WallBuild] = new WallBuild(prices);

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                _canWall.Clear();
                var colonies = s_humans[param.playerId].GetEdifices(EdificeGroupId.Colony);

                for (int i = colonies.Count - 1; i >= 0; i--)
                {
                    if (colonies[i].CanWallBuild())
                        _canWall.Add(i);
                }

                if (_canWall.Count > 0)
                {
                    int index = _canWall.Rand();
                    s_humans[param.playerId].BuyWall(colonies[index], _cost);
                }
            }

            public override void Clear()
            {
                s_spells[Military][MilitarySpellId.WallBuild] = null;
            }
        }
    }
}
