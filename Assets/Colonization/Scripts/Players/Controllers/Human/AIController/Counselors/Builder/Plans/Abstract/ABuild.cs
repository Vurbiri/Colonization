using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            private abstract class ABuild : Plan
            {
                public delegate WaitSignal Build(Crossroad crossroad, ReadOnlyMainCurrencies cost);

                protected readonly Crossroad _crossroad;
                private readonly ReadOnlyMainCurrencies _cost;
                private readonly Build _build;

                protected ABuild(Builder parent, Crossroad crossroad, ReadOnlyMainCurrencies cost, Build build) : base(parent)
                {
                    _crossroad = crossroad; _cost = cost; _build = build;
                }

                sealed public override IEnumerator Execution_Cn()
                {
                    if (!_done)
                    {
                        yield return Human.Exchange(_cost);

                        if (CanPlay)
                        {
                            yield return GameContainer.CameraController.ToPositionControlled(_crossroad);
                            yield return _build(_crossroad, _cost);
                            _done = true;
                        }
                    }
                    yield return s_waitRealtime.Restart();
                }
            }
        }
    }
}
