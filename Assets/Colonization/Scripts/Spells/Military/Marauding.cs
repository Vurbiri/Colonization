using System;
using System.Collections.Generic;
using System.Text;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Marauding : ASpell
        {
            private readonly CurrenciesLite[] _currencies = new CurrenciesLite[PlayerId.HumansCount];
            private readonly Stack<Occupation> _occupations = new(CONST.DEFAULT_MAX_EDIFICES << 1);
            private string _text;

            private Marauding(int type, int id) : base(type, id)
            {
                Localization.Instance.Subscribe(SetText);
                _currencies = new CurrenciesLite[PlayerId.HumansCount];
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _currencies[i] = new();
            }
            public static void Create() => new Marauding(MilitarySpellId.Type, MilitarySpellId.Marauding);

            public override bool Prep(SpellParam param)
            {
                _occupations.Clear(); 
                var human = s_humans[param.playerId];
                if (human.IsPay(_cost) & human.Actors.Count > 0)
                {
                    List<Hexagon> hexagons;
                    for (int playerId = 0; playerId < PlayerId.HumansCount; playerId++)
                    {
                        if (GameContainer.Diplomacy.GetRelation(param.playerId, playerId) == Relation.Enemy)
                        {
                            var colonies = s_humans[playerId].GetEdifices(EdificeGroupId.Colony);
                            for(int c = colonies.Count - 1; c >= 0; c--)
                            {
                                hexagons = colonies[c].Hexagons;
                                for (int h = 0; h < Crossroad.HEX_COUNT; h++)
                                    if (hexagons[h].IsOwner)
                                        _occupations.Push(new(hexagons[h].Owner, colonies[c]));
                            }
                        }
                    }
                }
                return _canCast = _occupations.Count > 0;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    Occupation.self = _currencies[param.playerId];
                    bool isPerson = param.playerId == PlayerId.Person;

                    while (_occupations.Count > 0)
                        isPerson |= _occupations.Pop().Heist(_currencies);

                    s_humans[param.playerId].Pay(_cost);

                    if (isPerson)
                    {
                        StringBuilder sb = new(200); sb.AppendLine(_text); Occupation.self.MainToStringBuilder(sb);
                        int amount = _currencies[PlayerId.Person].Amount;
                        Banner.Open(sb.ToString(), amount == 0 ? MessageTypeId.Warning : amount > 0 ? MessageTypeId.Profit : MessageTypeId.Error, 15f);
                    }

                    for (int i = 0; i < PlayerId.HumansCount; i++)
                    {
                        s_humans[i].AddResources(_currencies[i]);
                        _currencies[i].Clear();
                    }

                    _canCast = false;
                }
            }

            public override void Clear(int type, int id)
            {
                Localization.Instance.Unsubscribe(SetText);
                Occupation.self = null;
                s_spells[type][id] = null;
            }

            private void SetText(Localization localization) => _text = localization.GetText(s_settings.maraudingText);

            #region Nested: Occupation
            // =====================================================
            private class Occupation
            {
                private readonly Actor _actor;
                private readonly Crossroad _colony;

                public static CurrenciesLite self;

                public Occupation(Actor actor, Crossroad colony)
                {
                    _actor = actor;
                    _colony = colony;
                }

                public bool Heist(CurrenciesLite[] currencies)
                {
                    int enemyId = _colony.Owner;
                    int currencyId = _actor.Hexagon.GetProfit();
                    int currency = s_humans[enemyId].Resources[currencyId];
                    var enemy = currencies[enemyId];

                    if (currency > -enemy[currencyId] && Chance.Rolling(100 - s_settings.reductionFromWall * _colony.GetDefense()))
                    {
                        currency = Math.Min(currency, s_settings.maraudingCount[_actor.Id]);
                        enemy.Add(currencyId, -currency);
                        self.Add(currencyId, currency);

                        GameContainer.Diplomacy.Marauding(enemyId, _actor.Owner);
                    }

                    return enemyId == PlayerId.Person;
                }
            }
            // =====================================================
            #endregion
        }
    }
}
