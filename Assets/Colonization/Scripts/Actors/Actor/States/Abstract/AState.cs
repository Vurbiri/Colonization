using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.FSMSelectable;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class AState : ASelectableState
        {
            protected readonly Actor _actor;
            protected readonly ActorSkin _skin;

            protected bool ActorInteractable
            {
                [Impl(256)] set => _actor.Interactable = value;
            }

            public AState(Actor parent) : base(parent._stateMachine)
            {
                _actor = parent;
                _skin = parent._skin;
            }

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _actor.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine routine) => _actor.StopCoroutine(routine);
        }
    }
}
