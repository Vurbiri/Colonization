using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class SummonWarlock : ASharedSpell
        {
            private readonly int _max;
            private readonly Hexagons _hexagons;

            private SummonWarlock(Hexagons hexagons)
            {
                _hexagons = hexagons;
                _max = hexagons.GroundCount / 3;
                Debug.Log($"GroundCount: {hexagons.GroundCount}");
            }

            public static void Create(Hexagons hexagons) => s_sharedSpells[TypeOfPerksId.Economic][EconomicSpellId.SummonWarlock] = new SummonWarlock(hexagons);

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                if (s_humans[param.playerId].IsMaxWarriors)
                    return false;

                int count = 0;
                for (int i = 0; i < PlayerId.Count; i++)
                    count += s_actors[i].Count;

                if (count == _hexagons.GroundCount) 
                    return false;

                Hexagon hexagon;
                if (count <= _max)
                {
                    while (!(hexagon = _hexagons[HEX.NEARS.Rand().Rand()]).CanWarriorEnter) ;
                }
                else
                {
                    List<Hexagon> free = new(_hexagons.GroundCount - count);
                    ReadOnlyCollection<Key> keys;
                    for (int i = 0; i < 3; i++)
                    {
                        keys = HEX.NEARS[i];
                        for (int j = keys.Count - 1; j >= 0; j--)
                        {
                            hexagon = _hexagons[keys[j]];
                            if (hexagon.CanWarriorEnter)
                                free.Add(hexagon);
                        }
                    }
                    hexagon = free.Rand();
                }

                s_humans[param.playerId].RecruitingFree(WarriorId.Warlock, hexagon);

                return true;
            }
        }
    }
}
