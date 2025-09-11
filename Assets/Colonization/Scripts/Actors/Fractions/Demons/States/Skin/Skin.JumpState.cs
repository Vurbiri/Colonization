using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class FattySkin
    {
        sealed private class JumpState : ASkinState
        {
            private readonly FattySFX _sfx;
            private readonly WaitScaledTime _waitHit;
            private readonly WaitScaledTime _waitEnd;

            public readonly WaitSignal signal = new();

            public JumpState(ActorSkin parent, FattySFX sfx, AnimationTime timing) : base(parent)
            {
                _sfx = sfx;
                _waitHit = timing.WaitHits[0];
                _waitEnd = timing.WaitEnd;
            }

            public override void Enter()
            {
                EnableAnimation(s_idSpecSkill);
                StartCoroutine(Run_Cn());
            }

            public override void Exit() => DisableAnimation(s_idSpecSkill);

            private IEnumerator Run_Cn()
            {
                yield return _waitHit.Restart();
                _sfx.Spec(Skin);
                signal.Send();

                yield return _waitEnd.Restart();
                signal.Send();
            }
        }
    }
}
