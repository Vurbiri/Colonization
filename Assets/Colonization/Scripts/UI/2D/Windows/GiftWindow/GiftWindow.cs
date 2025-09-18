using TMPro;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public partial class GiftWindow : MonoBehaviour
    {
        [SerializeField] private FileIdAndKey _keyOk;
        [SerializeField] private FileIdAndKey _keyCancel;
        [Space]
        [SerializeField] private Switcher _switcher;
        [Space]
        [SerializeField] private TextMeshProUGUI _opponentName;
        [Space]
        [SerializeField] private TextMeshProUGUI _playerAmount;
        [Space]
        [SerializeField] private HintButton _applyButton;
        [SerializeField] private HintButton _resetButton;
        [SerializeField] private SimpleButton _closeButton;
        [Space]
        [SerializeField, ReadOnly] private PlayerCurrencyWidget[] _playerCurrencies;

        private readonly string[] _ok = new string[PlayerId.AICount], _cancel = new string[PlayerId.AICount];
        private readonly CurrenciesLite _gift = new();
        private Id<PlayerId> _currentPlayer = PlayerId.None;

        public Switcher Init(GiftButton[] giftButtons)
        {
            _switcher.Init(this);
            _switcher.onClose.Add(ResetValues);

            _applyButton.Init(Apply);
            _resetButton.Init(ResetValues);
            _closeButton.AddListener(_switcher.Close);

            Debug.Log("GiftWindow - заменить true на false");
            for(int i = giftButtons.Length - 1; i >= 0; i--)
                giftButtons[i].Init(Switch, true);

            var resources = GameContainer.Players.Person.Resources;
            for (int i = 0; i < CurrencyId.MainCount; i++)
                _playerCurrencies[i].Init(resources, OnChangeCount);

            Localization.Instance.Subscribe(SetLocalizationText);

            return _switcher;
        }

        private void Switch(Id<PlayerId> player)
        {
            if(_currentPlayer != player)
            {
                _currentPlayer = player;
                _opponentName.text = GameContainer.UI.PlayerNames[player];
                _opponentName.color = GameContainer.UI.PlayerColors[player];
                _switcher.Open();
            }
            else
            {
                _switcher.Switch();
            }
        }

        private void OnChangeCount(int id, int value)
        {
            _gift.SetMain(id, value);

            bool isNotZero = _gift.Amount > 0;
            _playerAmount.text = _gift.Amount.ToString();

            _resetButton.Interactable = isNotZero;
            _applyButton.Interactable = isNotZero;
        }

        private void Apply()
        {
            if (GameContainer.Players.Humans[_currentPlayer].Gift(PlayerId.Person, _gift))
            {
                GameContainer.Players.Person.Pay(_gift);
                Banner.Open(_ok[_currentPlayer - PlayerId.AI_01], MessageTypeId.Profit, 5f, true);
                _switcher.Close();
            }
            else
            {
                Banner.Open(_cancel[_currentPlayer - PlayerId.AI_01], MessageTypeId.Error, 5f, true);
            }
        }

        private void ResetValues()
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
            {
                _playerCurrencies[i].ResetCount();
                _gift.DirtyReset(i);
            }
            _gift.ResetAmount();

            _playerAmount.text = "0";
            _resetButton.Interactable = _applyButton.Interactable = false;
        }

        private void SetLocalizationText(Localization localization)
        {
            var ok = localization.GetText(_keyOk);
            var cancel = localization.GetText(_keyCancel);

            string name;
            for (int i = 0; i < PlayerId.AICount; i++)
            {
                name = GameContainer.UI.PlayerNames[i + PlayerId.AI_01];
                _ok[i] = string.Format(ok, name);
                _cancel[i] = string.Format(cancel, name);
            }
        }

        private void OnDestroy()
        {
            Localization.Instance.Unsubscribe(SetLocalizationText);
        }
    }
}
