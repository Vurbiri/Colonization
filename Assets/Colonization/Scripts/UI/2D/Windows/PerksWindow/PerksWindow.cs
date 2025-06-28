using System;
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
        [SerializeField] private HintButton _learnButton;
        [SerializeField] private SimpleButton _closeButton;
        [Space]
        [SerializeField] private IdSet<TypeOfPerksId, PerkTreeProgressBar> _progressBars;
        [Space]
        [SerializeField] private Color _colorLearn;

        private Action _onOpen;
        private Human _player;

        public void Init(Human player, CanvasHint hint, Action onOpen)
        {
            _switcher.Init(this);

            _player = player;
            _onOpen = onOpen;

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

        public void Close()
        {
            _switcher.Switch(false);
            SetAllTogglesOff();
        }
        public void Open()
        {
            _switcher.Switch(true);
            _onOpen();
        }
        public void Switch()
        {
            if (_switcher.Switch())
                _onOpen();
            else
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
