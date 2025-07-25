using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class WallBuild : ASpell
        {
            private readonly List<Crossroad> _canWall = new(CONST.DEFAULT_MAX_EDIFICES);

            private WallBuild(int type, int id) : base(type, id)
            {
                _cost.Add(GameContainer.Prices.Wall);
            }
            public static void Create() => new WallBuild(TypeOfPerksId.Military, MilitarySpellId.WallBuild);

            public override bool Prep(SpellParam param)
            {
                _canWall.Clear();
                if (s_humans[param.playerId].IsPay(_cost))
                {
                    var colonies = s_humans[param.playerId].GetEdifices(EdificeGroupId.Colony);
                    for (int i = colonies.Count - 1; i >= 0; i--)
                    {
                        if (colonies[i].CanWallBuild())
                            _canWall.Add(colonies[i]);
                    }
                }
                return _canCast = _canWall.Count > 0;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_humans[param.playerId].BuyWall(_canWall.Rand(), _cost);
                    _canCast = false;
                }
            }

        }
    }
}
