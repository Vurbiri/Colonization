using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class DeathState : ASkinState
        {
            private readonly WaitSignal _waitStartAnimation = new();
            private readonly WaitRealtime _waitEndAnimation;
            
            public readonly WaitStateController<Actor.DeathState> waitState = new(Actor.DeathState.None);

            public DeathState(ActorSkin parent, float duration) : base(B_DEATH, parent)
            {
                _waitEndAnimation = new(duration);

                _animator.GetBehaviour<DeathBehaviour>().EventEnter += OnEventEnter;
            }

            public override void Enter()
            {
                _animator.SetBool(_idParam, true);
                _sfx.Death();
                _parent.StartCoroutine(Death_Cn());
            }

            private IEnumerator Death_Cn()
            {
                waitState.SetState(Actor.DeathState.Start);
                yield return _waitStartAnimation;
                yield return _waitEndAnimation;
                waitState.SetState(Actor.DeathState.Animation);
                yield return _sfx.Death_Cn();
                waitState.SetState(Actor.DeathState.SFX);
            }

            private void OnEventEnter()
            {
                _animator.SetBool(_idParam, false);
                _waitStartAnimation.Send();
            }
        }
    }
}
