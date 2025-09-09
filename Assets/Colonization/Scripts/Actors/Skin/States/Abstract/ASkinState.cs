using System.Collections;
using UnityEngine;
using Vurbiri.FSM;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class ASkinState : AState
        {
            private readonly ActorSkin _parent;

            protected ActorSkin Skin { [Impl(256)] get => _parent; }
            protected ActorSFX SFX { [Impl(256)] get => _parent._sfx; }
            protected Animator Animator { [Impl(256)] get => _parent._animator; }
            protected Enumerator WaitEndAnimation { [Impl(256)] get => _parent._durationDeath; }

            [Impl(256)] public ASkinState(ActorSkin parent) : base(parent._stateMachine)
            {
                _parent = parent;
            }

            [Impl(256)] protected DeathBehaviour GetDeathBehaviour() => _parent._animator.GetBehaviour<DeathBehaviour>();
            [Impl(256)] protected ReactBehaviour[] GeReactBehaviours() => _parent._animator.GetBehaviours<ReactBehaviour>();

            [Impl(256)] protected void EnableAnimation(int idParam) => _parent._animator.SetBool(idParam, true);
            [Impl(256)] protected void DisableAnimation(int idParam) => _parent._animator.SetBool(idParam, false);

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _parent.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine routine) => _parent.StopCoroutine(routine);
        }
    }
}
