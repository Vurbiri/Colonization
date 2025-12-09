using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public class PlayerInteractable : ReactiveValue<bool>
    {
        private const int SPELL_INDEX = 26, TURN_INDEX = 28;

        protected readonly Id<PlayerId> _id;
        private int _code;

        public PlayerInteractable(Id<PlayerId> playerId, Subscription subscriptions)
        {
            _id = playerId;

            subscriptions += GameContainer.GameEvents.Subscribe(BindTurn);
            subscriptions += SpellBook.IsCasting.Subscribe(BindSpells);
            subscriptions += GameContainer.Actors[playerId].Subscribe(BindActors);
        }

        private void BindTurn(Id<GameModeId> gameMode, TurnQueue turn)
        {
            SetValue(TURN_INDEX, gameMode == GameModeId.Play && turn.currentId == _id);
        }
        private void BindTurn(bool value)
        {
            SetValue(TURN_INDEX, value);
        }
        private void BindSpells(bool cast)
        {
            SetValue(SPELL_INDEX, !cast);
        }

        private void BindActors(Actor actor, TypeEvent op)
        {
            if (op == TypeEvent.Subscribe | op == TypeEvent.Add)
                actor.InteractableReactive.Subscribe(value => SetValue(actor.Index, value));
            else if (op == TypeEvent.Remove)
                SetValue(actor.Index, true);
        }

        private void SetValue(int index, bool value)
        {
            _code = value ? _code & ~(1 << index) : _code | (1 << index);

            value = _code == 0;
            if (_value ^ value)
                _onChange.Invoke(_value = value);
        }
    }
}
