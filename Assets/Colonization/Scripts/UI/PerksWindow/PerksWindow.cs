using System.Collections;
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
        private Coroutine _coroutine;

        protected override void Start()
        {
            base.Start();

            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0f;
        }

        public void Init(Human player, CanvasHint hint)
        {
            _player = player;

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

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            SetAllTogglesOff();
            _canvasGroup.blocksRaycasts = open;
            _coroutine = StartCoroutine(Switch_Cn(open ? 1f : 0f));

            #region Local: Close_Cn()
            //=================================
            IEnumerator Switch_Cn(float end)
            {
                float start = _canvasGroup.alpha;
                float progress = 0f;

                while (progress < 0)
                {
                    progress += Time.unscaledDeltaTime * _speedSwitch;
                    _canvasGroup.alpha = start + (end - start) * progress;
                    yield return null;
                }

                _canvasGroup.alpha = end;
                _coroutine = null;
            }
            #endregion
        }
    }
}
