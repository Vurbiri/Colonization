using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class DemonSpecMoveSkin 
    {
        sealed private class SpecMoveState : ASpecState
        {
            public SpecMoveState(ActorSkin parent, DemonSFX sfx) : base("bWalk_Spec", parent, sfx) { }

            public override void Enter() => StartCoroutine(Run_Cn());

            private IEnumerator Run_Cn()
            {
                yield return SFX.Spec(Skin);
                EnableAnimation();
                signal.Send();
            }
        }
    }
}
