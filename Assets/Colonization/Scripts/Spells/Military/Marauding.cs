using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Marauding : ASharedSpell
        {
            private readonly Diplomacy _diplomacy;
            private readonly CurrenciesLite[] _currencies = new CurrenciesLite[PlayerId.HumansCount];

            private Marauding() 
            {
                _diplomacy = Player.States.diplomacy;
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _currencies[i] = new();
            }
            public static void Create() => s_sharedSpells[TypeOfPerksId.Military][MilitarySpellId.Marauding] = new Marauding();

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                if(s_actors[param.playerId].Count == 0)
                    return false;

                int enemyId, currencyId; float ratioChance;
                CurrenciesLite enemyRes;

                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _currencies[i].Clear();

                foreach (var actor in s_actors[param.playerId])
                {
                    foreach (var crossroad in actor.Hexagon.Crossroads)
                    {
                        enemyId = crossroad.Owner;
                        if (crossroad.IsColony && _diplomacy.GetRelation(param.playerId, enemyId) == Relation.Enemy)
                        {
                            enemyRes = _currencies[enemyId];
                            ratioChance = 1f - s_settings.reductionFromWall * crossroad.GetDefense();
                            currencyId = actor.Hexagon.GetProfit();

                            if (s_humans[enemyId].Resources[currencyId] > enemyRes[currencyId] && Chance.Rolling(Mathf.RoundToInt(s_settings.chanceMarauding[actor.Id] * ratioChance)))
                            {
                                enemyRes.Add(currencyId, -1);
                                resources.Add(currencyId, 1);
                            }
                        }
                    }
                }

                for (int i = 0; i < PlayerId.HumansCount; i++)
                    s_humans[i].AddResources(_currencies[i]);

               return true;
            }
        }
    }
}
