using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            protected abstract class AActionState : AState
            {
                public readonly AbilityValue costAP;
                public readonly SkillCode code;
                public readonly WaitSignal signal = new();

                #region Propirties
                protected SubAbility<ActorAbilityId> HP { [Impl(256)] get => _parent._actor._HP; }
                protected SubAbility<ActorAbilityId> AP { [Impl(256)] get => _parent._actor._AP; }
                protected EffectsSet ActorEffects { [Impl(256)] get => _parent._actor._effects; }
                #endregion

                public virtual bool CanUse { [Impl(256)] get => AP >= costAP.Value; }

                public AActionState(AStates<TActor, TSkin> parent, int id, int cost) : base(parent)
                {
                    code = new(parent._actor._typeId, parent._actor._id, id);
                    costAP = new(TypeModifierId.Addition, cost);
                }

                sealed public override void Cancel() => Unselect(null);

                public override void Exit()
                {
                    StopCoroutine();
                    signal.Send();
                }

                [Impl(256)]
                protected void Pay()
                {
                    Moving.Off();
                    AP.RemoveModifier(costAP);
                    Actor.ChangeSignal();
                }
            }
        }
    }
}
