using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vurbiri.Colonization.Actors;
using static Vurbiri.Colonization.TypeOfPerksId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class SummonWarlock : ASpell
        {
            private readonly int _max;
            private readonly CurrenciesLite _cost;

            private SummonWarlock(Prices prices)
            {
                _max = s_hexagons.GroundCount >> 1;
                _cost = new(prices.Warriors[WarriorId.Warlock])
                {
                    { CurrencyId.Mana, s_costs[Economic][EconomicSpellId.SummonWarlock] }
                };
            }

            public static void Create(Prices prices) => s_economicSpells[EconomicSpellId.SummonWarlock] = new SummonWarlock(prices);

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                if (s_humans[param.playerId].IsMaxWarriors)
                    return;

                int count = 0;
                for (int i = 0; i < PlayerId.Count; i++)
                    count += s_actors[i].Count;

                if (count == s_hexagons.GroundCount) 
                    return;

                Hexagon hexagon;
                if (count <= _max)
                {
                    while (!(hexagon = s_hexagons[HEX.NEARS.Random]).CanWarriorEnter);
                }
                else
                {
                    List<Hexagon> free = new(s_hexagons.GroundCount - count);
                    ReadOnlyCollection<Key> keys;
                    for (int i = 0; i < HEX.NEARS.Count; i++)
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

                s_humans[param.playerId].Recruiting(WarriorId.Warlock, hexagon, _cost);

                s_cameraController.ToPosition(hexagon.Position);
            }

            public override void Clear()
            {
                s_economicSpells[EconomicSpellId.SummonWarlock] = null;
            }
        }
    }
}
