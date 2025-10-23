using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class DemonSpecMoveSkin 
    {
        sealed private class SpecMoveState : ASpecState
        {
            public SpecMoveState(ActorSkin parent, DemonSFX sfx) : base(parent, sfx) { }

            public override void Enter() => StartCoroutine(Run_Cn());
            public override void Exit() => DisableAnimation(s_idSpecMove);

            private IEnumerator Run_Cn()
            {
                yield return SFX.Spec(Skin);
                EnableAnimation(s_idSpecMove);
                signal.Send();
            }
        }
    }
}
