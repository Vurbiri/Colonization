//Assets\Colonization\Scripts\Actors\Skin\States\DeathState.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class DeathState : ASkinState
        {
            private readonly WaitForSecondsRealtime _waitEndAnimation;

            public readonly WaitActivate waitActivate = new();

            public DeathState(ActorSkin parent, float duration) : base(B_DEATH, parent)
            {
                _waitEndAnimation = new(duration);
            }

            public override void Enter()
            {
                _animator.SetBool(_idParam, true);
                _sfx.Death();
                _parent.StartCoroutine(Death_Cn());
            }

            private IEnumerator Death_Cn()
            {
                yield return new WaitForSecondsRealtime(0.1f);
                _animator.SetBool(_idParam, false);
                yield return _waitEndAnimation;
                yield return _sfx.Death_Cn();
                waitActivate.Activate();
            }
        }
    }
}
