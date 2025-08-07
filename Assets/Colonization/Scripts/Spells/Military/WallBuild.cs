using System.Collections.Generic;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class WallBuild : ASpell
        {
            private readonly List<Crossroad> _canWall = new(CONST.DEFAULT_MAX_EDIFICES);
            private readonly string _strCost;

            private WallBuild(int type, int id) : base(type, id)
            {
                _cost.Add(GameContainer.Prices.Wall);
                _strCost = _cost.MainPlusToString();
            }
            public static void Create() => new WallBuild(MilitarySpellId.Type, MilitarySpellId.WallBuild);

            public override bool Prep(SpellParam param)
            {
                _canWall.Clear();
                if (!s_isCast && s_humans[param.playerId].IsPay(_cost))
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
                    var colony = _canWall.Rand();
                    s_humans[param.playerId].BuyWall(colony, _cost);

                    GameContainer.CameraController.ToPosition(colony);
                    ShowNameSpell(param.playerId);

                    _canCast = false;
                }
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
