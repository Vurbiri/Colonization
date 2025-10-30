using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.FSMSelectable;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            protected abstract class AState : ASelectableState
            {
                protected readonly AStates<TActor, TSkin> _parent;

                #region Propirties
                protected TActor Actor { [Impl(256)] get => _parent._actor; }
                protected TSkin Skin { [Impl(256)] get => _parent._skin; }
                protected BooleanAbility<ActorAbilityId> Moving { [Impl(256)] get => _parent._actor._move; }
                protected RBool IsCancel { [Impl(256)] get => _parent._actor._canCancel; }
                protected bool IsPerson { [Impl(256)] get => _parent._actor._isPersonTurn; }

                protected Hexagon CurrentHex
                {
                    [Impl(256)] get => _parent._actor._currentHex;
                    [Impl(256)] set => _parent._actor._currentHex = value;
                }

                protected Transform Transform { [Impl(256)] get => _parent._actor._thisTransform; }
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
                }

                [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _parent._actor.StartCoroutine(routine);
                [Impl(256)] protected void StopCoroutine(Coroutine routine) => _parent._actor.StopCoroutine(routine);
            }
        }
    }
}
