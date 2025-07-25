namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Marauding : ASpell
        {
            private readonly CurrenciesLite[] _currencies = new CurrenciesLite[PlayerId.HumansCount];

            private Marauding(int type, int id) : base(type, id)
            {
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _currencies[i] = new();
            }
            public static void Create() => new Marauding(TypeOfPerksId.Military, MilitarySpellId.Marauding);

            public override bool Prep(SpellParam param)
            {
                var human = s_humans[param.playerId];
                return _canCast = human.IsPay(_cost) & human.Actors.Count > 0;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    int enemyId, currencyId, chance;
                    CurrenciesLite enemy, self = _currencies[param.playerId];

                    for (int i = 0; i < PlayerId.HumansCount; i++)
                        _currencies[i].Clear();

                    foreach (var actor in s_actors[param.playerId])
                    {
                        foreach (var crossroad in actor.Hexagon.Crossroads)
                        {
                            enemyId = crossroad.Owner;
                            if (crossroad.IsColony && GameContainer.Diplomacy.GetRelation(param.playerId, enemyId) == Relation.Enemy)
                            {
                                enemy = _currencies[enemyId];
                                chance = 100 - s_settings.reductionFromWall * crossroad.GetDefense();
                                currencyId = actor.Hexagon.GetProfit();

                                if (s_humans[enemyId].Resources[currencyId] > -enemy[currencyId] && Chance.Rolling(chance))
                                {
                                    enemy.Add(currencyId, -1);
                                    self.Add(currencyId, 1);
                                }
                            }
                        }
                    }

                    s_humans[param.playerId].Pay(_cost);
                    for (int i = 0; i < PlayerId.HumansCount; i++)
                        s_humans[i].AddResources(_currencies[i]);

                    _canCast = false;
                }
            }
        }
    }
}
