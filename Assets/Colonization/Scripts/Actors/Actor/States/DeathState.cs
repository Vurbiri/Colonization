using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        sealed protected class DeathState : AState
        {
            public WaitStateSource<DeathStage> stage;

            public DeathState(Actor parent) : base(parent) { }
            
            public override void Enter()
            {
                stage = _skin.Death();

                _actor.Removing();
                _actor.StartCoroutine(Death_Cn(stage.SetWaitState(DeathStage.SFX)));
            }

            IEnumerator Death_Cn(WaitState<DeathStage> wait)
            {
                yield return wait;
                Destroy(_actor.gameObject);
            }
        }
    }
}
