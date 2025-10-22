using System.Collections;
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
                _strCost = _cost.PlusToString(SEPARATOR);
            }
            public static void Create() => new WallBuild(MilitarySpellId.Type, MilitarySpellId.WallBuild);

            public override bool Prep(SpellParam param)
            {
                if (_canCast = !s_isCasting)
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
                    s_isCasting.True();
                    Cast_Cn(param.playerId, _canWall.Rand()).Start();

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(int playerId, Crossroad colony)
            {
                yield return GameContainer.CameraController.ToPositionControlled(playerId, colony.Position);
                ShowSpellName(playerId);
                yield return Humans[playerId].BuyWall(colony, _cost);
                s_isCasting.False();
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
