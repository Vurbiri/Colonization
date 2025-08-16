using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class AActionState : AState
        {
            protected readonly bool _isPlayer;
            private readonly AbilityValue _costAP;

            #region Propirties
            private SubAbility<ActorAbilityId> AP { [Impl(256)] get => _actor._currentAP; }
            protected BooleanAbility<ActorAbilityId> Moving { [Impl(256)] get => _actor._move; }

            protected RBool IsCancel { [Impl(256)] get => _actor._canCancel; }

            protected Vector3 ActorPosition
            {
                [Impl(256)] get => _actor._thisTransform.localPosition;
                [Impl(256)] set => _actor._thisTransform.localPosition = value;
            }
            protected Quaternion ActorRotation
            {
                [Impl(256)] set => _actor._thisTransform.localRotation = value;
            }

            protected Hexagon ActorHex 
            { 
                [Impl(256)] get => _actor._currentHex;
                [Impl(256)] set => _actor._currentHex = value;
            }
            #endregion

            public AActionState(Actor parent, int cost = 0) : base(parent)
            {
                _isPlayer = parent._owner == PlayerId.Person;
                _costAP = new(TypeModifierId.Addition, cost);
            }

            public override void Cancel() => Unselect(null);

            protected virtual void Pay()
            {
                Moving.Off();
                AP.RemoveModifier(_costAP);
            }
        }
    }
}
