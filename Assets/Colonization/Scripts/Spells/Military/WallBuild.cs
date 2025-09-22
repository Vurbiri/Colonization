using System.Collections.Generic;
using Vurbiri.International;

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
                _strCost = _cost.MainPlusToString(SEPARATOR);
            }
            public static void Create() => new WallBuild(MilitarySpellId.Type, MilitarySpellId.WallBuild);

            public override bool Prep(SpellParam param)
            {
                if (_canCast = !s_isCast)
                {
                    _canWall.Clear();
                    if (Humans[param.playerId].IsPay(_cost))
                    {
                        var colonies = Humans[param.playerId].GetEdifices(EdificeGroupId.Colony);
                        for (int i = colonies.Count - 1; i >= 0; i--)
                        {
                            if (colonies[i].CanWallBuild())
                                _canWall.Add(colonies[i]);
                        }
                    }
                    _canCast = _canWall.Count > 0;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    var colony = _canWall.Rand();
                    Humans[param.playerId].BuyWall(colony, _cost);

                    GameContainer.CameraController.ToPosition(colony);
                    ShowSpellName(param.playerId);

                    _canCast = false;
                }
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
