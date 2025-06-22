using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public partial class PerksWindow : VToggleGroup<PerkToggle>
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _speedSwitch = 8f;
        [Space]
        [SerializeField] private HintButton _perksButton;
        [SerializeField] private HintButton _closeButton;
        [SerializeField] private HintButton _learnButton;
        [Space]
        [SerializeField] private IdSet<TypeOfPerksId, PerkTreeProgressBar> _progressBars;
        [Space]
        [SerializeField] private Color _colorLearn;

        private Human _player;
        private WaitSwitchFloat _waitLerp;

        protected override void Start()
        {
            base.Start();

            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
        }

        public void Init(Human player, CanvasHint hint)
        {
            _player = player;

            _waitLerp = new(0f, 1f, _speedSwitch, _canvasGroup.GetSetor<float>("alpha"));

            _perksButton.Init(hint, Switch);
            _closeButton.Init(hint, Close);
            _learnButton.Init(hint, OnLearn);

            PerkTree perkTree = player.Perks;
            var blood = player.Resources.Get(CurrencyId.Blood);

            foreach (var perk in _toggles)
                perk.Init(perkTree, blood, hint, _colorLearn);

            for (int i = 0; i < TypeOfPerksId.Count; i++)
                _progressBars[i].Init(perkTree.GetProgress(i));

            _onValueChanged.Add(toggle => _learnButton.Interactable = toggle != null, _activeToggle);

            _progressBars = null; _perksButton = _closeButton = null;
        }

        private void OnLearn()
        {
            if (_activeToggle)
            {
                _activeToggle.BuyPerk(_player, _colorLearn);
            }
        }
                
        private void Close()
        {
            Switch(false);
        }
        private void Switch()
        {
            Switch(!_canvasGroup.blocksRaycasts);
        }

        private void Switch(bool open)
        {
            if (_canvasGroup.blocksRaycasts == open)
                return;

            if (_waitLerp.IsRunning)
                StopCoroutine(_waitLerp);

            SetAllTogglesOff();
            _canvasGroup.blocksRaycasts = open;
            StartCoroutine(open ? _waitLerp.Forward(_canvasGroup.alpha) : _waitLerp.Backward(_canvasGroup.alpha));
        }
    }
}
