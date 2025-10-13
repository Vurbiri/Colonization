namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class WallBuild : ABuild
            {
                public override bool IsValid => true;

                private WallBuild(Builder parent, Crossroad crossroad) : base(parent, crossroad, GameContainer.Prices.Wall, parent.Human.BuyWall) { }

                public static void Create(Builder parent, Plans plans)
                {
                    if (parent.Human.IsWallUnlock())
                    {
                        var baseWeight = s_settings.wallWeight + parent.GetCostWeight(GameContainer.Prices.Wall);
                        var colonies = parent.Colonies;
                        Crossroad colony;

                        for (int i = 0, weight; i < colonies.Count; i++)
                        {
                            colony = colonies[i];
                            if (colony.CanWallBuild())
                            {
                                weight = baseWeight + colony.Weight;
                                if (weight > 0)
                                    plans.Add(new WallBuild(parent, colony), weight);
                            }
                        }
                    }
                }
            }
        }
    }
}
