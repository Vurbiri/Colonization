using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class DeathState : AState
            {
                public WaitStateSource<DeathStage> stage;

                public DeathState(AStates<TActor, TSkin> parent) : base(parent) { }

                public override void Enter()
                {
                    stage = Skin.Death();

                    Actor.Removing();

                    StartCoroutine(Death_Cn(stage.SetWaitState(DeathStage.End)));
                }

                IEnumerator Death_Cn(WaitState<DeathStage> wait)
                {
                    yield return wait;
                    Destroy(Actor.gameObject);
                }
            }
        }
    }
}
