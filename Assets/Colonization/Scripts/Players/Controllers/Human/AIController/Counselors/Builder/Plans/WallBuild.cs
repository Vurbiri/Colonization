using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class WallBuild : ABuild
            {
                public override bool IsValid => true;

                private WallBuild(Builder parent, Crossroad crossroad, ReadOnlyMainCurrencies cost, int weight) : base(parent, crossroad, cost, weight, parent.Human.BuyWall) { }

                public static void Create(Builder parent, List<Plan> plans, Crossroad crossroad)
                {
                    var cost = GameContainer.Prices.Wall;
                    int weight = crossroad.Weight + s_settings.wallWeight + parent.GetCostWeight(cost);
                    if (weight > 0)
                        plans.Add(new WallBuild(parent, crossroad, cost, weight + plans[^1].Weight));
                }
            }
        }
    }
}
