using Vurbiri.Colonization.Characteristics;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            protected abstract class AActionState : AState
            {
                private readonly AbilityValue _costAP;

                public readonly SkillCode code;
                public readonly WaitSignal signal = new();

                #region Propirties
                protected SubAbility<ActorAbilityId> HP { [Impl(256)] get => _parent._actor._currentHP; }
                protected SubAbility<ActorAbilityId> AP { [Impl(256)] get => _parent._actor._currentAP; }
                protected EffectsSet ActorEffects { [Impl(256)] get => _parent._actor._effects; }
                public bool CanUse { [Impl(256)] get => AP >= _costAP.Value; }
                 #endregion

                public AActionState(AStates<TActor, TSkin> parent, int id, int cost) : base(parent)
                {
                    code = new(parent._actor._typeId, parent._actor._id, id);
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
