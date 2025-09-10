using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    sealed public class PersonController : AHumanController
    {
        private readonly InteractableController _iController;

        public PersonController(Settings settings) : base(PlayerId.Person, settings)
        {
            _iController = new(_interactable);
            _spellBook.IsCast.Subscribe(_iController.BindSpells);
            Actors.Subscribe(_iController.BindActors);
        }

        public override void OnEndLanding()
        {
            _edifices.Interactable = false;
            _iController.Turn = false;
        }

        public override void OnPlay()
        {
            _edifices.Interactable = true;

            foreach (var warrior in Actors)
            {
                warrior.IsPersonTurn = true;
                warrior.Interactable = true;
            }

            _iController.Turn = true;
        }

        public override void OnEndTurn()
        {
            _iController.Turn = false;
            _edifices.Interactable = false;

            base.OnEndTurn();
        }

        #region Nested InteractableController
        //**********************************************************************************
        private class InteractableController
        {
            private readonly RBool _change;

            private int _actors;
            private bool _spells, _isTurn = true;

            private bool Value
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _actors == 0 & _spells & _isTurn;
            }

            public bool Turn 
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set
                {
                    _isTurn = value;
                    _change.Value = Value;
                }
            }

            public InteractableController(RBool change) => _change = change;

            public void BindSpells(bool cast)
            {
                _spells = !cast;
                _change.Value = Value;
            }

            public void BindActors(Actor actor, TypeEvent op)
            {
                if(op == TypeEvent.Subscribe | op == TypeEvent.Add)
                    actor.InteractableReactive.Subscribe(value => SetActors(actor.Index, value));
                else if(op == TypeEvent.Remove)
                    SetActors(actor.Index, true);
            }

            private void SetActors(int index, bool value)
            {
                if (value) _actors &= ~(1 << index);
                else _actors |= 1 << index;

                _change.Value = Value;
            }
        }
        #endregion
    }
}
