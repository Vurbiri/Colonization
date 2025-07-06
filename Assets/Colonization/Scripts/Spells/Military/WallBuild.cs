using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class WallBuild : ASharedSpell
        {
            private readonly List<int> _canWall = new(CONST.DEFAULT_MAX_EDIFICES);

            private WallBuild() { }
            public static void Create() => s_sharedSpells[TypeOfPerksId.Military][MilitarySpellId.WallBuild] = new WallBuild();

            public override bool Cast(SpellParam param, CurrenciesLite resources)
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
                    if (colonies[index].BuildWall(param.playerId, true))
                    {
                        colonies.Signal(index);
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
