using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class Upgrade : Plan
            {
                private readonly Crossroad _crossroad;
                private readonly ReadOnlyMainCurrencies _cost;

                public override bool IsValid => true;
 
                public Upgrade(Builder parent, Crossroad crossroad, int weight) : base(parent) 
                {
                    int next = crossroad.NextId;

                    _crossroad = crossroad;
                    _cost = GameContainer.Prices.Edifices[next];
                    _weight = weight + s_settings.edificeWeight[next];
                }

                public override IEnumerator Appeal_Cn()
                {
                    if (!_done && Controller.Exchange(_cost))
                    {
                        yield return GameContainer.CameraController.ToPositionControlled(_crossroad);
                        yield return Controller.BuyEdificeUpgrade(_crossroad, _cost);
                        _done = true;
                    }
                    yield break;
                }
            }
        }
    }
}
