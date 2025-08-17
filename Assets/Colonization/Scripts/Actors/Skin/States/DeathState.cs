using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        sealed protected class DeathState : ASkinState
        {
            private readonly WaitSignal _waitStartAnimation = new();
            private readonly WaitRealtime _waitEndAnimation;
            
            public readonly WaitStateController<Actor.DeathStage> waitState = new(Actor.DeathStage.None);

            public DeathState(ActorSkin parent, float duration) : base(B_DEATH, parent)
            {
                _waitEndAnimation = new(duration);

                GetDeathBehaviour().EventEnter += OnEventEnter;
            }

            public override void Enter()
            {
                EnableAnimation();
                SFX.Death();
                StartCoroutine(Death_Cn());
            }

            private IEnumerator Death_Cn()
            {
                waitState.SetState(Actor.DeathStage.Start);
                yield return _waitStartAnimation;
                yield return _waitEndAnimation;
                waitState.SetState(Actor.DeathStage.Animation);
                yield return SFX.Death_Cn();
                waitState.SetState(Actor.DeathStage.SFX);
            }

            private void OnEventEnter()
            {
                DisableAnimation();
                _waitStartAnimation.Send();
            }
        }
    }
}
