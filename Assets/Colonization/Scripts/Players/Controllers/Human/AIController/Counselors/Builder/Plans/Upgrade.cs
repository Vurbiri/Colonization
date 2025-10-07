using System.Collections.Generic;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class Upgrade : ABuild
            {
                public override bool IsValid => true;

                private Upgrade(Builder parent, Crossroad crossroad, ReadOnlyMainCurrencies cost, int weight) : base(parent, crossroad, cost, weight, parent.Human.BuyEdificeUpgrade) { }

                public static void Create(Builder parent, List<Plan> plans, ReadOnlyReactiveList<Crossroad> edifice)
                {
                    for (int i = 0; i < edifice.Count; i++)
                        Upgrade.Create(parent, plans, edifice[i]);
                }

                private static void Create(Builder parent, List<Plan> plans, Crossroad crossroad)
                {
                    if (parent.Human.IsEdificeUnlock(crossroad.NextId) & crossroad.IsUpgrade)
                    {
                        var cost = GameContainer.Prices.Edifices[crossroad.NextId];
                        int weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(cost);
                        if (crossroad.NextGroupId == EdificeGroupId.Colony)
                            weight += parent.GetProfitWeight(crossroad.Hexagons);
                        if (weight > 0)
                            plans.Add(new Upgrade(parent, crossroad, cost, weight + plans[^1].Weight));
                    }
                }
            }
        }
    }
}
