using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class BossSkin
    {
        sealed private class EatState : ASkinState
        {
            private readonly BossSFX _sfx;
            private readonly WaitScaledTime[] _waitHits;
            private readonly WaitScaledTime _waitEnd;

            public readonly WaitSignal signal = new();

            public EatState(ActorSkin parent, BossSFX sfx, AnimationTime timing) : base(parent)
            {
                _sfx = sfx;
                _waitHits = timing.WaitHits;
                _waitEnd = timing.WaitEnd;
            }

            public override void Enter()
            {
                EnableAnimation(s_idSpecSkill);
                StartCoroutine(Run_Cn());
            }

            public override void Exit() => DisableAnimation(s_idSpecSkill);

            private IEnumerator Run_Cn()
            {
                yield return _waitHits[0].Restart();
                _sfx.SpecSkillStart();

                yield return _waitHits[1].Restart();
                _sfx.SpecSkill(Skin);

                signal.Send();

                yield return _waitEnd.Restart();
                signal.Send();
            }
        }
    }
}
