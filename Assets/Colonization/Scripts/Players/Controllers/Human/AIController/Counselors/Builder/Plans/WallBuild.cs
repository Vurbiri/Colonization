using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class WallBuild : Plan
            {
                private readonly Crossroad _crossroad;

                public override bool IsValid => true;

                public WallBuild(Builder parent, Crossroad crossroad, int weight) : base(parent)
                {
                    _crossroad = crossroad;
                    _weight = weight + s_settings.wallWeight;
                }

                public override IEnumerator Appeal_Cn()
                {
                    var cost = GameContainer.Prices.Wall;
                    if (!_done && Controller.Exchange(cost))
                    {
                        yield return GameContainer.CameraController.ToPositionControlled(_crossroad);
                        yield return Controller.BuyWall(_crossroad, cost);
                        _done = true;
                    }
                    yield break;
                }
            }
        }
    }
}
