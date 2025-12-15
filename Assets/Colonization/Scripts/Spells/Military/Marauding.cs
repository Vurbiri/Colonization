using System.Collections.Generic;
using System.Text;
using Vurbiri.Collections;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Marauding : AMsgSpell
        {
            private readonly LiteCurrencies[] _currencies = new LiteCurrencies[PlayerId.HumansCount];
            private readonly Stack<Occupation> _occupations = new(CONST.DEFAULT_MAX_EDIFICES << 1);

            private Marauding(int type, int id) : base(type, id)
            {
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _currencies[i] = new();

                SetManaCost();
            }
            public static void Create() => new Marauding(MilitarySpellId.Type, MilitarySpellId.Marauding);

            public override bool Prep(SpellParam param)
            {
                if (_canCast = !s_isCasting)
                {
                    _occupations.Clear();
                    var human = Humans[param.playerId];
                    if (human.IsPay(_cost) & human.Actors.Count > 0)
                    {
                        ReadOnlyArray<Hexagon> hexagons;
                        for (int playerId = 0; playerId < PlayerId.HumansCount; playerId++)
                        {
                            if (GameContainer.Diplomacy.IsEnemy(param.playerId, playerId))
                            {
                                var colonies = Humans[playerId].GetEdifices(EdificeGroupId.Colony);
                                for (int c = 0; c < colonies.Count; ++c)
                                {
                                    hexagons = colonies[c].Hexagons;
                                    for (int h = 0; h < Crossroad.HEX_COUNT; h++)
                                        if (hexagons[h].IsOwner(param.playerId))
                                            _occupations.Push(new(colonies[c], hexagons[h].GetProfit()));
                                }
                            }
                        }
                    }
                    _canCast = _occupations.Count > 0;
                }
                return _canCast;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    bool isPerson = param.playerId == PlayerId.Person;

                    while (_occupations.Count > 0)
                        isPerson |= _occupations.Pop().Heist(_currencies, param.playerId);

                    Humans[param.playerId].Pay(_cost);

                    if (isPerson)
                    {
                        StringBuilder sb = new(200); 
                        sb.AppendLine(_strMsg);
                        _currencies[param.playerId].ToStringBuilder(sb);
                        int amount = _currencies[PlayerId.Person].Amount;
                        Banner.Open(sb.ToString(), amount == 0 ? MessageTypeId.Warning : amount > 0 ? MessageTypeId.Profit : MessageTypeId.Error, 15f);
                    }

                    for (int i = 0; i < PlayerId.HumansCount; i++)
                    {
                        Humans[i].Resources.Add(_currencies[i]);
                        _currencies[i].Clear();
                    }

                    _canCast = false;
                }
            }

            protected override string GetDesc(Localization localization)
            {
                SetMsg(localization);

                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.reductionFromWall), _strCost);
            }

            #region Nested: Occupation
            // =====================================================
            private class Occupation
            {
                private readonly Crossroad _colony;
                private readonly Id<CurrencyId> _profit;

                public Occupation(Crossroad colony, Id<CurrencyId> profit)
                {
                    _colony = colony;
                    _profit = profit;
                }

                public bool Heist(LiteCurrencies[] currencies, Id<PlayerId> playerId)
                {
                    int enemyId = _colony.Owner;
                    int currency = Humans[enemyId].Resources[_profit];
                    var enemy = currencies[enemyId];
                    var self = currencies[playerId];

                    if (currency > -enemy[_profit] && Chance.Rolling(100 - s_settings.reductionFromWall * _colony.GetDefense()))
                    {
                        enemy.Decrement(_profit);
                        self.Increment(_profit);

                        GameContainer.Diplomacy.Marauding(enemyId, playerId);
                    }

                    return enemyId == PlayerId.Person;
                }
            }
            // =====================================================
            #endregion
        }
    }
}
