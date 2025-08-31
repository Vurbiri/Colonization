using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.FSMSelectable;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            protected abstract class AState : ASelectableState
            {
                protected readonly AStates<TActor, TSkin> _parent;

                protected TActor Actor { [Impl(256)] get => _parent._actor; }
                protected TSkin Skin { [Impl(256)] get => _parent._skin; }
   
                public AState(AStates<TActor, TSkin> parent) : base(parent._stateMachine)
                {
                    _parent = parent;
                }

                [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _parent._actor.StartCoroutine(routine);
                [Impl(256)] protected void StopCoroutine(Coroutine routine) => _parent._actor.StopCoroutine(routine);
            }
        }
    }
}
