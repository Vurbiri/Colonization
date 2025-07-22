using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Marauding : ASpell
        {
            private readonly CurrenciesLite[] _currencies = new CurrenciesLite[PlayerId.HumansCount];

            private Marauding() 
            {
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _currencies[i] = new();
            }
            public static void Create() => s_spells[TypeOfPerksId.Military][MilitarySpellId.Marauding] = new Marauding();

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                if(s_actors[param.playerId].Count == 0)
                    return;

                int enemyId, currencyId; float ratioChance;
                CurrenciesLite temp;

                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _currencies[i].Clear();

                foreach (var actor in s_actors[param.playerId])
                {
                    foreach (var crossroad in actor.Hexagon.Crossroads)
                    {
                        enemyId = crossroad.Owner;
                        if (crossroad.IsColony && GameContainer.Diplomacy.GetRelation(param.playerId, enemyId) == Relation.Enemy)
                        {
                            temp = _currencies[enemyId];
                            ratioChance = 1f - s_settings.reductionFromWall * crossroad.GetDefense();
                            currencyId = actor.Hexagon.GetProfit();

                            if (s_humans[enemyId].Resources[currencyId] > -temp[currencyId] && Chance.Rolling(Mathf.RoundToInt(s_settings.chanceMarauding[actor.Id] * ratioChance)))
                            {
                                temp.Add(currencyId, -1);
                                resources.Add(currencyId, 1);
                            }
                        }
                    }
                }

                temp = _currencies[param.playerId];
                _currencies[param.playerId] = resources;

                for (int i = 0; i < PlayerId.HumansCount; i++)
                    s_humans[i].AddResources(_currencies[i]);

                _currencies[param.playerId] = temp;
            }

            public override void Clear()
            {
                s_spells[TypeOfPerksId.Military][MilitarySpellId.Marauding] = null;
            }
        }
    }
}
