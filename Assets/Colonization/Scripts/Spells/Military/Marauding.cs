using System.Collections.Generic;
using System.Text;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Marauding : AMsgSpell
        {
            private readonly MainCurrencies[] _currencies = new MainCurrencies[PlayerId.HumansCount];
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
                if (_canCast = !s_isCast)
                {
                    _occupations.Clear();
                    var human = Humans[param.playerId];
                    if (human.IsPay(_cost) & human.Actors.Count > 0)
                    {
                        List<Hexagon> hexagons;
                        for (int playerId = 0; playerId < PlayerId.HumansCount; playerId++)
                        {
                            if (GameContainer.Diplomacy.GetRelation(param.playerId, playerId) == Relation.Enemy)
                            {
                                var colonies = Humans[playerId].GetEdifices(EdificeGroupId.Colony);
                                for (int c = colonies.Count - 1; c >= 0; c--)
                                {
                                    hexagons = colonies[c].Hexagons;
                                    for (int h = 0; h < Crossroad.HEX_COUNT; h++)
                                        if (hexagons[h].IsOwner)
                                            _occupations.Push(new(hexagons[h].Owner, colonies[c]));
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
                    Occupation.self = _currencies[param.playerId];
                    bool isPerson = param.playerId == PlayerId.Person;

                    while (_occupations.Count > 0)
                        isPerson |= _occupations.Pop().Heist(_currencies);

                    Humans[param.playerId].Pay(_cost);

                    if (isPerson)
                    {
                        StringBuilder sb = new(200); 
                        sb.AppendLine(_strMsg); Occupation.self.ToStringBuilder(sb);
                        int amount = _currencies[PlayerId.Person].Amount;
                        Banner.Open(sb.ToString(), amount == 0 ? MessageTypeId.Warning : amount > 0 ? MessageTypeId.Profit : MessageTypeId.Error, 15f);
                    }

                    for (int i = 0; i < PlayerId.HumansCount; i++)
                    {
                        Humans[i].AddResources(_currencies[i]);
                        _currencies[i].Clear();
                    }

                    _canCast = false;
                    Occupation.self = null;
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
                private readonly Actor _actor;
                private readonly Crossroad _colony;

                public static MainCurrencies self;

                public Occupation(Actor actor, Crossroad colony)
                {
                    _actor = actor;
                    _colony = colony;
                }

                public bool Heist(MainCurrencies[] currencies)
                {
                    int enemyId = _colony.Owner;
                    int currencyId = _actor.Hexagon.GetProfit();
                    int currency = Humans[enemyId].Resources[currencyId];
                    var enemy = currencies[enemyId];

                    if (currency > -enemy[currencyId] && Chance.Rolling(100 - s_settings.reductionFromWall * _colony.GetDefense()))
                    {
                        enemy.Decrement(currencyId);
                        self.Increment(currencyId);

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
