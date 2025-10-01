using System.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class PersonController : HumanController
    {
        private readonly InteractableController _iController;

        public PersonController(Settings settings) : base(PlayerId.Person, settings)
        {
            _iController = new(_interactable, _subscription);
        }

        public override WaitResult<bool> Gift(int giver, CurrenciesLite gift, string msg)
        {
            Gift_Cn(giver, gift, msg).Start();
            return _waitGift.Restart();

            // Local
            IEnumerator Gift_Cn(int giver, CurrenciesLite gift, string msg)
            {
                yield return MessageBox.Open(msg, out WaitButton wait, MBButton.OkNo);

                bool result = wait.Id == MBButtonId.Ok;
                if (result)
                {
                    _resources.Add(gift);
                    GameContainer.Diplomacy.Gift(_id, giver);
                }

                _waitGift.SetResult(result);
            }
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

            OnEndTurn_Cn().Start();
        }

        #region Nested InteractableController
        //**********************************************************************************
        private class InteractableController
        {
            private readonly RBool _change;

            private int _actors;
            private bool _spells, _isTurn = true;

            private bool Value  { [Impl(256)] get => _actors == 0 & _spells & _isTurn; }

            public bool Turn 
            {
                [Impl(256)] set
                {
                    _isTurn = value;
                    _change.Value = Value;
                }
            }

            public InteractableController(RBool change, Subscription subscriptions)
            {
                _change = change;

                UnityEngine.Debug.LogWarning("[PersonController.InteractableController] Uncomment");
                //_isTurn = GameContainer.GameLoop.IsPersonTurn;
                subscriptions += SpellBook.IsCast.Subscribe(BindSpells);
                subscriptions += GameContainer.Actors[PlayerId.Person].Subscribe(BindActors);
            }

            public void BindSpells(bool cast)
            {
                _spells = !cast;
                _change.Value = Value;
            }

            private void BindActors(Actor actor, TypeEvent op)
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
