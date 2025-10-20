using System.Collections;
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
                _strCost = _cost.PlusToString(SEPARATOR);
            }

            public static void Create() => new SummonWarlock(EconomicSpellId.Type, EconomicSpellId.SummonWarlock);

            public override bool Prep(SpellParam param)
            {
                _canCast = false;
                var human = Humans[param.playerId];
                if (!s_isCasting & !human.IsMaxWarriors && human.IsPay(_cost))
                {
                    _count = 0;
                    for (int i = 0; i < PlayerId.Count; i++)
                        _count += GameContainer.Actors[i].Count;

                    _canCast = _count < _max;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_isCasting.True();
                    Cast_Cn(param.playerId).Start();

                    _canCast = false;
                }
            }

            private IEnumerator Cast_Cn(int playerId)
            {
                Hexagon hexagon;
                if (_count <= _half)
                {
                    while (!(hexagon = GameContainer.Hexagons[HEX.NEARS.Random]).CanWarriorEnter);
                }
                else
                {
                    List<Hexagon> free = new(_max - _count); ReadOnlyArray<Key> keys;
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

                yield return null;

                GameContainer.CameraController.ToPositionControlled(playerId, hexagon);
                ShowSpellName(playerId);
                yield return Humans[playerId].Recruiting(WarriorId.Warlock, hexagon, _cost);

                s_isCasting.False();
            }

            protected override string GetDesc(Localization localization) => string.Concat(localization.GetText(FILE, _descKey), _strCost);
        }
    }
}
