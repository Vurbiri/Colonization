using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
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

        private Human _player;

        public ISubscription OnOpen => _switcher.onOpen;
        public ISubscription OnClose => _switcher.onClose;

        public void Init(Human player, CanvasHint hint, bool open)
        {
            _switcher.Init(this, open);
            _switcher.onClose.Add(SetAllTogglesOff);

            _player = player;

            _learnButton.Init(hint, OnLearn);
            _closeButton.AddListener(Close);

            PerkTree perkTree = player.Perks;
            var blood = player.Resources.Get(CurrencyId.Blood);

            foreach (var perk in _toggles)
                perk.Init(perkTree, blood, hint, _colorLearn);

            for (int i = 0; i < TypeOfPerksId.Count; i++)
                _progressBars[i].Init(perkTree.GetProgress(i));


            _onValueChanged.Add(toggle => _learnButton.Interactable = toggle != null, _activeToggle);

            _progressBars = null; _closeButton = null;
        }

        public void Close() => _switcher.Switch(false);
        public void Open() => _switcher.Switch(true);
        public void Switch() => _switcher.Switch();

        private void OnLearn()
        {
            if (_activeToggle)
            {
                _activeToggle.BuyPerk(_player, _colorLearn);
            }
        }
    }
}
