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

                private WallBuild(Builder parent, Crossroad crossroad, int weight) : base(parent, crossroad, GameContainer.Prices.Wall, weight, parent.Human.BuyWall) { }

                public static void Create(Builder parent, List<Plan> plans)
                {
                    if (parent.Human.IsWallUnlock())
                    {
                        var baseWeight = s_settings.wallWeight + parent.GetCostWeight(GameContainer.Prices.Wall);
                        var colonies = parent.Colonies;

                        for (int i = 0, weight; i < colonies.Count; i++)
                        {
                            weight = baseWeight + colonies[i].Weight;
                            if (weight > 0)
                                plans.Add(new WallBuild(parent, colonies[i], weight + plans[^1].Weight));
                        }
                    }
                }
            }
        }
    }
}
