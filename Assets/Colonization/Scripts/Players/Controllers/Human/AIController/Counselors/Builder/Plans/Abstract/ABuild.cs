using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            private abstract class ABuild : Plan
            {
                public delegate WaitSignal Build(Crossroad crossroad, ReadOnlyLiteCurrencies cost);

                protected readonly Crossroad _crossroad;
                private readonly ReadOnlyLiteCurrencies _cost;
                private readonly Build _build;

                protected ABuild(Builder parent, Crossroad crossroad, ReadOnlyLiteCurrencies cost, Build build) : base(parent)
                {
                    _crossroad = crossroad; _cost = cost; _build = build;
                }

                sealed public override IEnumerator Execution_Cn()
                {
                    if (!_done)
                    {
                        yield return Human.Exchange_Cn(_cost, Out<bool>.Get(out int key));

                        if (Out<bool>.Result(key))
                        {
                            yield return GameContainer.CameraController.ToPositionControlled(_crossroad.Position);
                            yield return _build(_crossroad, _cost);
                            _done = true;
#if TEST_AI
                            UnityEngine.Debug.Log($"[Builder::{this}] {HumanId} {_build.Method.Name}");
#endif
                        }
                    }
                    yield return s_delayHalfSecond.Restart();
                }
            }
        }
    }
}
