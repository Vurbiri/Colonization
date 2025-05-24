using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class AActionState : AState
        {
            protected readonly bool _isPlayer;
            protected readonly BooleanAbility<ActorAbilityId> _move;
            private readonly SubAbility<ActorAbilityId> _currentAP;
            private readonly AbilityValue _costAP;
            
            public AActionState(Actor parent, int cost = 0, int id = 0) : base(parent, id)
            {
                _isPlayer = parent._owner == PlayerId.Player;
                _move = parent._move;
                _currentAP = parent._currentAP;
                _costAP = new(TypeModifierId.Addition, cost);
            }
            public AActionState(Actor parent, int cost, TypeIdKey key) : base(parent, key)
            {
                _isPlayer = parent._owner == PlayerId.Player;
                _move = parent._move;
                _currentAP = parent._currentAP;
                _costAP = new(TypeModifierId.Addition, cost);
            }

            public override void Cancel() => Unselect(null);

            protected virtual void Pay()
            {
                _move.Off();
                _currentAP.RemoveModifier(_costAP);
            }
        }
    }
}
