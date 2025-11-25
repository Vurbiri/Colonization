using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class PortBuild : ABuild
            {
                public override bool IsValid => _crossroad.CanBuild(HumanId);

                private PortBuild(Builder parent, Crossroad crossroad, ReadOnlyMainCurrencies cost) : base(parent, crossroad, cost, parent.Human.BuyEdificeUpgrade) { }

                public static void Create(Builder parent, Plans plans)
                {
                    if (parent.Abilities.IsGreater(HumanAbilityId.MaxPort, parent.Ports.Count) && GameContainer.Crossroads.CoastCount > 0)
                    {
                        var crossroad = GetPort();
                        var cost = GameContainer.Prices.Edifices[crossroad.NextId];
                        int weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(cost);
                        if (weight > 0)
                            plans.Add(new PortBuild(parent, crossroad, cost), weight);
                    }
                }

                public static Crossroad GetPort()
                {
                    var crossroads = GameContainer.Crossroads;
                    List<Crossroad> ports = new();
                    Crossroad port;

                    while (crossroads.TryExtractPort(out port))
                    {
                        if (port.IsNearBuildings())
                            ports.Add(port);
                        else
                            break;
                    }
#if TEST_AI
                    Log.Info($"[Builder][PortBuild] count of rejected ports [{ports.Count}]");
#endif
                    crossroads.ReturnPorts(ports);

                    return port ?? crossroads.GetRandomPort();
                }
            }
        }
    }
}
