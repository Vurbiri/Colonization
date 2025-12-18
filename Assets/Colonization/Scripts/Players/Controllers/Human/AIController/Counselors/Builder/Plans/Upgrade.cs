using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class Upgrade : ABuild
            {
                private Upgrade(Builder parent, Crossroad crossroad, ReadOnlyLiteCurrencies cost) : base(parent, crossroad, cost, parent.Human.BuyEdificeUpgrade) { }

                public static void Create(Builder parent, Plans plans, ReadOnlyReactiveList<Crossroad> edifice)
                {
                    for (int i = 0; i < edifice.Count; i++)
                        Upgrade.Create(parent, plans, edifice[i]);
                }

                private static void Create(Builder parent, Plans plans, Crossroad crossroad)
                {
                    if (parent.Human.IsEdificeUnlock(crossroad.NextId) & crossroad.IsUpgrade)
                    {
                        var cost = GameContainer.Prices.Edifices[crossroad.NextId];
                        int weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(cost);
                        if (crossroad.NextGroupId == EdificeGroupId.Colony)
                            weight += parent.GetProfitWeight(crossroad.Hexagons);
                        if (weight > 0)
                            plans.Add(new Upgrade(parent, crossroad, cost), weight);
                    }
                }
            }
        }
    }
}
