using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public partial class PerksWindow : VToggleGroup<PerkToggle>, IWindow
    {
        [SerializeField] private Switcher _switcher;
        [Space]
        [SerializeField] private HintButton _learnButton;
        [SerializeField] private SimpleButton _closeButton;
        [Space]
        [SerializeField] private IdSet<TypeOfPerksId, PerkTreeProgressBar> _progressBars;
        [Space]
        [SerializeField] private Color _colorLearn;

        public ISubscription OnOpen => _switcher.onOpen;
        public ISubscription OnClose => _switcher.onClose;

        public void Init(bool open)
        {
            _switcher.Init(this, open);
            _switcher.onClose.Add(SetAllTogglesOff);

            _learnButton.Init(OnLearn);
            _closeButton.AddListener(Close);

            var person = GameContainer.Players.Person;
            var perkTree = person.Perks;
            var blood = person.Resources.Get(CurrencyId.Blood);

            foreach (var perk in _toggles)
                perk.Init(perkTree, blood, _colorLearn);

            for (int i = 0; i < TypeOfPerksId.Count; i++)
                _progressBars[i].Init(perkTree.GetProgress(i));


            _onValueChanged.Add(OnValueChanged, _activeToggle);

            _progressBars = null; _closeButton = null;
        }

        public void Close() => _switcher.Switch(false);
        public void Open() => _switcher.Switch(true);
        public void Switch() => _switcher.Switch();

        private void OnValueChanged(PerkToggle toggle)
        {
            _learnButton.Interactable = toggle != null;
        }
        private void OnLearn()
        {
            if (_activeToggle)
                _activeToggle.BuyPerk(_colorLearn);
        }
    }
}
