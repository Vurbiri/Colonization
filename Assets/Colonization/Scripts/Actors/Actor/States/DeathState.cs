using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        sealed protected class DeathState : AState<ActorSkin>
        {
            public WaitStateSource<DeathStage> stage;

            public DeathState(Actor parent) : base(parent, parent._skin) { }
            
            public override void Enter()
            {
                stage = _skin.Death();

                _actor.Removing();

                StartCoroutine(Death_Cn(stage.SetWaitState(DeathStage.SFX)));
            }

            IEnumerator Death_Cn(WaitState<DeathStage> wait)
            {
                yield return wait;
                Destroy(_actor.gameObject);
            }
        }
    }
}
