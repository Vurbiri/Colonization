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
                _parent.StartCoroutine(Death_Coroutine());
            }

            private IEnumerator Death_Coroutine()
            {
                yield return null;
                _animator.SetBool(_idParam, false);
                yield return _waitEndAnimation;
                yield return _sfx.Death_Coroutine();
                waitActivate.Activate();
            }
        }
    }
}
