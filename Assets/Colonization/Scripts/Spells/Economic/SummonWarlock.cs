using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vurbiri.Colonization.Actors;
using static Vurbiri.Colonization.HEX;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class SummonWarlock : ASharedSpell
        {
            private readonly int _max;

            private SummonWarlock()
            {
                _max = s_hexagons.GroundCount / 2;
            }

            public static void Create() => s_sharedSpells[TypeOfPerksId.Economic][EconomicSpellId.SummonWarlock] = new SummonWarlock();

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                if (s_humans[param.playerId].IsMaxWarriors)
                    return false;

                int count = 0;
                for (int i = 0; i < PlayerId.Count; i++)
                    count += s_actors[i].Count;

                if (count == s_hexagons.GroundCount) 
                    return false;

                Hexagon hexagon;
                if (count <= _max)
                {
                    while (!(hexagon = s_hexagons[HEX.NEARS.Random]).CanWarriorEnter);
                }
                else
                {
                    List<Hexagon> free = new(s_hexagons.GroundCount - count);
                    ReadOnlyCollection<Key> keys;
                    for (int i = 0; i < NEARS.Count; i++)
                    {
                        keys = HEX.NEARS[i];
                        for (int j = keys.Count - 1; j >= 0; j--)
                        {
                            hexagon = s_hexagons[keys[j]];
                            if (hexagon.CanWarriorEnter)
                                free.Add(hexagon);
                        }
                    }
                    hexagon = free.Rand();
                }

                s_humans[param.playerId].RecruitingFree(WarriorId.Warlock, hexagon);
                s_cameraController.ToPosition(hexagon.Position);

                return true;
            }
        }
    }
}
