using System.Collections;
using System.Text;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class PersonController : HumanController
    {
        private readonly PersonSettings _settings;
        private readonly Unsubscriptions _subscriptions;
        private readonly InteractableController _iController;
        private string _giftMsg;

        public PersonController(Settings settings) : base(PlayerId.Person, settings)
        {
            _settings = SettingsFile.Load<PersonSettings>();
            _iController = new(_interactable, _subscriptions);
            _subscriptions += Localization.Instance.Subscribe(SetLocalizationText);
        }

        public override WaitResult<bool> Gift(int giver, CurrenciesLite gift)
        {
            Gift_Cn(giver, gift).Start();
            return _waitGift.Restart();

            // Local
            IEnumerator Gift_Cn(int giver, CurrenciesLite gift)
            {
                StringBuilder sb = new(TAG.ALING_CENTER, 256);
                sb.Append(GameContainer.UI.PlayerNames[giver]); sb.Append(" "); sb.AppendLine(_giftMsg);
                gift.MainPlusToStringBuilder(sb); sb.Append(TAG.ALING_OFF);

                yield return MessageBox.Open(sb.ToString(), out WaitButton wait, MBButton.OkNo);

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

            base.OnEndTurn();
        }

        public override void Dispose()
        {
            _subscriptions.Unsubscribe();
            base.Dispose();
        }

        private void SetLocalizationText(Localization localization)
        {
            _giftMsg = localization.GetText(_settings.giftMsg);
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

            public InteractableController(RBool change, Unsubscriptions subscriptions)
            {
                _change = change;

                _isTurn = GameContainer.GameLoop.IsPersonTurn;
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
