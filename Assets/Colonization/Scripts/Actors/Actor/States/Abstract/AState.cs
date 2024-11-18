using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.FSMSelectable;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public class AState : ASelectableState
        {
            protected readonly Actor _actor;
            protected readonly ActorSkin _skin;
            protected readonly GameplayEventBus _eventBus;

            private readonly Ability<ActorAbilityId> _move;
            private readonly Ability<ActorAbilityId> _currentAP;
            private readonly AbilityModAddSettings _costModAP;

            public AState(Actor parent, int cost = 0, int id = 0) : base(parent._stateMachine, id)
            {
                _actor = parent;
                _skin = parent._skin;

                _move = parent._move;
                _currentAP = parent._currentAP;
                _costModAP = new(cost);

                _eventBus = parent._eventBus;
            }

            protected void MoveFalse() => _move.IsValue = false;
            protected void Pay()
            {
                _currentAP.RemoveModifier(_costModAP);
                _move.IsValue = false;
            }

        }
    }
}
