using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class BombSkin
    {
        sealed private class ExplosionState : ASpecState
        {
            private readonly WaitScaledTime _waitHit;
            private readonly WaitScaledTime _waitEnd;

            public ExplosionState(ActorSkin parent, DemonSFX sfx, AnimationTime timing) : base(parent, sfx) 
            {
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
                yield return SFX.Spec(Skin);
                yield return _waitEnd.Restart();
                signal.Send();
            }
        }
    }
}
