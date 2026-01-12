using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public class PlayerInteractable : Reactive<bool>
    {
        private const int SPELL_INDEX = CONST.DEFAULT_MAX_DEMONS + 1, TURN_INDEX = SPELL_INDEX + 1;

        protected readonly Id<PlayerId> _id;
        private int _flags;

        public PlayerInteractable(Id<PlayerId> playerId, ref Subscription subscriptions)
        {
            _id = playerId;

            subscriptions += GameContainer.GameEvents.Subscribe(BindTurn);
            subscriptions += SpellBook.IsCasting.Subscribe(BindSpells);
            subscriptions += GameContainer.Actors[playerId].Subscribe(BindActors);
        }

        private void BindTurn(Id<GameModeId> gameMode, TurnQueue turn)
        {
            SetValue(TURN_INDEX, gameMode == GameModeId.Play & turn.currentId == _id);
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

        private void SetValue(int row, bool value)
        {
            _flags = _flags.SetRow(row, !value);

            value = _flags == 0;
            if (_value ^ value)
                _onChange.Invoke(_value = value);
        }
    }
}
