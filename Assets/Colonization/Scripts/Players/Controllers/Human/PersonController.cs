using System;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    sealed public class PersonController : AHumanController
    {
        private readonly RInteractable _interactable = new();

        public IReactiveValue<bool> Interactable => _interactable;

        public PersonController(Settings settings) : base(PlayerId.Person, settings)
        {
            _spellBook.IsCast.Subscribe(_interactable.BindSpells);
            _actors.Subscribe(_interactable.BindActors);
        }

        public override void OnEndLanding()
        {
            _edifices.Interactable = false;
            _interactable.Turn = false;
        }

        public override void OnPlay()
        {
            _edifices.Interactable = true;

            foreach (var warrior in _actors)
            {
                warrior.IsPersonTurn = true;
                warrior.Interactable = true;
            }

            _interactable.Turn = true;
        }

        public override void OnEndTurn()
        {
            _interactable.Turn = false;
            _edifices.Interactable = false;

            base.OnEndTurn();
        }

        private class RInteractable : IReactiveValue<bool>
        {
            private int _actors;
            private bool _spells, _isTurn = true;

            private readonly Subscription<bool> _change = new();

            public bool Value
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _actors == 0 & _spells & _isTurn;
            }

            public bool Turn 
            {   
                set
                {
                    bool old = Value;
                    _isTurn = value;
                    if (old != Value)
                        _change.Invoke(!old);
                }
            }

            public void BindSpells(bool cast)
            {
                bool old = Value;
                _spells = !cast;
                if (old != Value) _change.Invoke(!old);
            }

            public void BindActors(Actor actor, TypeEvent op)
            {
                if(op == TypeEvent.Subscribe | op == TypeEvent.Add)
                    actor.InteractableReactive.Subscribe(value => SetActors(actor.Index, value));
                else if(op == TypeEvent.Remove)
                    SetActors(actor.Index, true);
            }

            public Unsubscription Subscribe(Action<bool> action, bool instantGetValue = true) => _change.Add(action, Value);

            public static implicit operator bool(RInteractable self) => self.Value;

            private void SetActors(int index, bool value)
            {
                bool old = Value;

                if (value) _actors &= ~(1 << index);
                else _actors |= 1 << index;

                if (old != Value) _change.Invoke(!old);
            }
        }
    }
}
