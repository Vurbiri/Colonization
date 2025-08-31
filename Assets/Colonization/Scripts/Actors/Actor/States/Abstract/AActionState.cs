using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            protected abstract class AActionState : AState
            {
                protected readonly bool _isPerson;
                private readonly AbilityValue _costAP;

                #region Propirties
                private SubAbility<ActorAbilityId> AP { [Impl(256)] get => _parent._actor._currentAP; }
                protected BooleanAbility<ActorAbilityId> Moving { [Impl(256)] get => _parent._actor._move; }

                protected RBool IsCancel { [Impl(256)] get => _parent._actor._canCancel; }

                protected Vector3 Position
                {
                    [Impl(256)] get => _parent._actor._thisTransform.localPosition;
                    [Impl(256)] set => _parent._actor._thisTransform.localPosition = value;
                }
                protected Quaternion Rotation
                {
                    [Impl(256)] set => _parent._actor._thisTransform.localRotation = value;
                }

                protected Hexagon CurrentHex
                {
                    [Impl(256)] get => _parent._actor._currentHex;
                    [Impl(256)] set => _parent._actor._currentHex = value;
                }
                protected EffectsSet ActorEffects
                {
                    [Impl(256)] get => _parent._actor._effects;
                }
                #endregion

                public AActionState(AStates<TActor, TSkin> parent, int cost = 0) : base(parent)
                {
                    _isPerson = parent._actor._owner == PlayerId.Person;
                    _costAP = new(TypeModifierId.Addition, cost);
                }

                sealed public override void Cancel() => Unselect(null);

                protected virtual void Pay()
                {
                    Moving.Off();
                    AP.RemoveModifier(_costAP);
                }
            }
        }
    }
}
