using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class BombSkin
    {
        sealed private class ExplosionState : ASpecAttackState
        {
            private readonly WaitScaledTime _waitHit;
            private readonly WaitScaledTime _waitEnd;

            public ExplosionState(ActorSkin parent, DemonSFX sfx, AnimationTime timing) : base(parent, sfx) 
            {
                _waitHit = timing.WaitHits[0];
                _waitEnd = timing.WaitEnd;
                UnityEngine.Debug.Log(_waitEnd.Time);
            }

            public override void Enter()
            {
                EnableAnimation();
                StartCoroutine(Run_Cn());
            }

            private IEnumerator Run_Cn()
            {
                yield return _waitHit;

                SFX.Spec(Skin);

                yield return _waitEnd;
                signal.Send();
            }
        }
    }
}
