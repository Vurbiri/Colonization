using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public partial class PerksWindow : VToggleGroup<PerkToggle>
    {
        [SerializeField] private Switcher _switcher;
        [Space]
        [SerializeField] private HintButton _closeButton;
        [SerializeField] private HintButton _learnButton;
        [Space]
        [SerializeField] private IdSet<TypeOfPerksId, PerkTreeProgressBar> _progressBars;
        [Space]
        [SerializeField] private Color _colorLearn;

        private Human _player;

        public void Init(Human player, CanvasHint hint)
        {
            _switcher.Init(this);

            _player = player;

            _closeButton.Init(hint, Close);
            _learnButton.Init(hint, OnLearn);

            PerkTree perkTree = player.Perks;
            var blood = player.Resources.Get(CurrencyId.Blood);

            foreach (var perk in _toggles)
                perk.Init(perkTree, blood, hint, _colorLearn);

            for (int i = 0; i < TypeOfPerksId.Count; i++)
                _progressBars[i].Init(perkTree.GetProgress(i));

            
            _onValueChanged.Add(toggle => _learnButton.Interactable = toggle != null, _activeToggle);

            _progressBars = null; _closeButton = null;
        }

        public void Close()
        {
            SetAllTogglesOff();
            _switcher.Switch(false);
        }
        public void Open()
        {
            _switcher.Switch(true);
        }
        public void Switch()
        {
            _switcher.Switch();
            if (!_switcher.Open)
                SetAllTogglesOff();
        }

        private void OnLearn()
        {
            if (_activeToggle)
            {
                _activeToggle.BuyPerk(_player, _colorLearn);
            }
        }
    }
}
