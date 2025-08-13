using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;

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
                _strCost = _cost.MainPlusToString(COST_COUNT_LINE);
            }

            public static void Create() => new SummonWarlock(EconomicSpellId.Type, EconomicSpellId.SummonWarlock);

            public override bool Prep(SpellParam param)
            {
                _canCast = false;
                var human = s_humans[param.playerId];
                if (!s_isCast & !human.IsMaxWarriors && human.IsPay(_cost))
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
                    Hexagon hexagon;
                    if (_count <= _half)
                    {
                        while (!(hexagon = GameContainer.Hexagons[HEX.NEARS.Random]).CanWarriorEnter);
                    }
                    else
                    {
                        List<Hexagon> free = new(_max - _count);
                        ReadOnlyArray<Key> keys;
                        for (int i = 0; i < HEX.NEARS.Count; i++)
                        {
                            keys = HEX.NEARS[i];
                            for (int j = 0; j < keys.Count; j++)
                            {
                                hexagon = GameContainer.Hexagons[keys[j]];
                                if (hexagon.CanWarriorEnter)
                                    free.Add(hexagon);
                            }
                        }
                        hexagon = free.Rand();
                    }

                    ShowSpellName(param.playerId);
                    s_humans[param.playerId].Recruiting(WarriorId.Warlock, hexagon, _cost);
                    _canCast = false;

                    GameContainer.CameraController.ToPosition(hexagon);
                }
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
