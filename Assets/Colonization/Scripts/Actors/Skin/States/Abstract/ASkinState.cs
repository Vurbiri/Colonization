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
            private readonly int _idParam;

            protected ActorSkin Skin { [Impl(256)] get => _parent; }
            protected ActorSFX SFX { [Impl(256)] get => _parent._sfx; }

            public ASkinState(string stateName, ActorSkin parent) : base(parent._stateMachine)
            {
                _parent = parent;
                _idParam = Animator.StringToHash(stateName);
            }

            [Impl(256)] protected DeathBehaviour GetDeathBehaviour() => _parent._animator.GetBehaviour<DeathBehaviour>();
            [Impl(256)] protected ReactBehaviour[] GeReactBehaviours() => _parent._animator.GetBehaviours<ReactBehaviour>();

            [Impl(256)] protected void EnableAnimation() => _parent._animator.SetBool(_idParam, true);
            [Impl(256)] protected void DisableAnimation() => _parent._animator.SetBool(_idParam, false);

            [Impl(256)] protected void SetTrigger() => _parent._animator.SetTrigger(_idParam);
            [Impl(256)] protected void ResetTrigger() => _parent._animator.ResetTrigger(_idParam);

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _parent.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine routine) => _parent.StopCoroutine(routine);
        }
    }
}
