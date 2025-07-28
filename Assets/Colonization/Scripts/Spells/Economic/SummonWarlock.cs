using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class SummonWarlock : ASpell
        {
            private readonly int _half, _max;
            private int _count;

            private SummonWarlock(int type, int id) : base(type, id)
            {
                _max = GameContainer.Hexagons.GroundCount;
                _half = _max - (_max >> 1);
                _cost.Add(GameContainer.Prices.Warriors[WarriorId.Warlock]);
            }

            public static void Create() => new SummonWarlock(EconomicSpellId.Type, EconomicSpellId.SummonWarlock);

            public override bool Prep(SpellParam param)
            {
                _canCast = false;
                var human = s_humans[param.playerId];
                if (!human.IsMaxWarriors & human.IsPay(_cost))
                {
                    _count = 0;
                    for (int i = 0; i < PlayerId.Count; i++)
                        _count += s_actors[i].Count;

                    _canCast = _count < _max;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    Hexagons hexagons = GameContainer.Hexagons; Hexagon hexagon;
                    if (_count <= _half)
                    {
                        while (!(hexagon = hexagons[HEX.NEARS.Random]).CanWarriorEnter) ;
                    }
                    else
                    {
                        List<Hexagon> free = new(_max - _count);
                        ReadOnlyCollection<Key> keys;
                        for (int i = 0; i < HEX.NEARS.Count; i++)
                        {
                            keys = HEX.NEARS[i];
                            for (int j = keys.Count - 1; j >= 0; j--)
                            {
                                hexagon = hexagons[keys[j]];
                                if (hexagon.CanWarriorEnter)
                                    free.Add(hexagon);
                            }
                        }
                        hexagon = free.Rand();
                    }

                    s_humans[param.playerId].Recruiting(WarriorId.Warlock, hexagon, _cost);
                    _canCast = false;

                    GameContainer.CameraController.ToPosition(hexagon);
                }
            }
        }
    }
}
