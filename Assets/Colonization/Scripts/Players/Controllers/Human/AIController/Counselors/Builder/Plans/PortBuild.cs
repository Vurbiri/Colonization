using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class PortBuild : ABuild
            {
                public override bool IsValid => _crossroad.CanBuild(Id);

                private PortBuild(Builder parent, Crossroad crossroad, ReadOnlyMainCurrencies cost, int weight) : base(parent, crossroad, cost, weight, parent.Human.BuyEdificeUpgrade) { }

                public static void Create(Builder parent, List<Plan> plans)
                {
                    if (parent.Abilities.IsGreater(HumanAbilityId.MaxPort, parent.Ports.Count) && GameContainer.Crossroads.ShoreCount > 0)
                    {
                        var crossroad = GameContainer.Crossroads.GetRandomPort();
                        var cost = GameContainer.Prices.Edifices[crossroad.NextId];
                        int weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(cost);
                        if (weight > 0)
                            plans.Add(new PortBuild(parent, crossroad, cost, weight + plans[^1].Weight));
                    }
                }
            }
        }
    }
}
