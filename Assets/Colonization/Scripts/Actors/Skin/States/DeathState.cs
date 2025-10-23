using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class ActorSkin
    {
        sealed protected class DeathState : ASkinState
        {
            private readonly WaitSignal _waitStartAnimation = new();
 
            public readonly WaitStateController<Actor.DeathStage> waitState = new(Actor.DeathStage.None);

            public DeathState(ActorSkin parent) : base(parent)
            {
                GetDeathBehaviour().EventEnter += OnEventEnter;
            }

            public override void Enter()
            {
                EnableAnimation(s_idDeath);
                SFX.Death();
                StartCoroutine(Death_Cn());
            }

            private IEnumerator Death_Cn()
            {
                waitState.SetState(Actor.DeathStage.Start);
                yield return _waitStartAnimation;
                yield return WaitEndAnimation;
                waitState.SetState(Actor.DeathStage.EndAnimation);
                yield return SFX.PostDeath(-1.1f * Skin._bounds.extents.z);
                waitState.SetState(Actor.DeathStage.End);
            }

            private void OnEventEnter()
            {
                DisableAnimation(s_idDeath);
                _waitStartAnimation.Send();
            }
        }
    }
}
