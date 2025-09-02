using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class DemonSpecMoveSkin 
    {
        sealed private class SpecMoveState : ASkinState
        {
            private readonly DemonSFX _sfx;

            public readonly WaitSignal signal = new();

            public SpecMoveState(ActorSkin parent, DemonSFX sfx) : base("bWalk_Spec", parent)
            {
                _sfx = sfx;
            }

            public override void Enter() => StartCoroutine(Run_Cn());

            public override void Exit() => DisableAnimation();

            private IEnumerator Run_Cn()
            {
                yield return _sfx.Spec(Skin);
                EnableAnimation();
                signal.Send();
            }
        }
    }
}
