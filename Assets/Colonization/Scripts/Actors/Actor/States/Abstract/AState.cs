using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;
using Vurbiri.Reactive;
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
                protected readonly bool _isPerson;

                #region Propirties
                protected TActor Actor { [Impl(256)] get => _parent._actor; }
                protected TSkin Skin { [Impl(256)] get => _parent._skin; }
                protected BooleanAbility<ActorAbilityId> Moving { [Impl(256)] get => _parent._actor._move; }
                protected RBool IsCancel { [Impl(256)] get => _parent._actor._canCancel; }
                protected Hexagon CurrentHex
                {
                    [Impl(256)] get => _parent._actor._currentHex;
                    [Impl(256)] set => _parent._actor._currentHex = value;
                }
                protected Vector3 Position
                {
                    [Impl(256)] get => _parent._actor._thisTransform.localPosition;
                    [Impl(256)] set => _parent._actor._thisTransform.localPosition = value;
                }
                protected Quaternion Rotation
                {
                    [Impl(256)] get => _parent._actor._thisTransform.localRotation;
                    [Impl(256)] set => _parent._actor._thisTransform.localRotation = value;
                }
                #endregion

                public AState(AStates<TActor, TSkin> parent) : base(parent._stateMachine)
                {
                    _parent = parent;
                    _isPerson = parent._actor._owner == PlayerId.Person;
                }

                [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _parent._actor.StartCoroutine(routine);
                [Impl(256)] protected void StopCoroutine(Coroutine routine) => _parent._actor.StopCoroutine(routine);
            }
        }
    }
}
