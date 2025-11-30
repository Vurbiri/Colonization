using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class SatanPanel : AHintElement
    {
        [SerializeField] private FileIdAndKey _hintKey;
        [Space]
        [SerializeField] private Image _indicator;
        [SerializeField, Range(0.1f, 2f)] private float _indicatorSpeed;
        [SerializeField] private WaitRealtime _waitStartIndicator;
        [Space]
        [SerializeField] private Image _bar;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _levelTMP;

        private bool _indicatorRun;
        private string _localizedText;

        public void Init()
        {
            base.InternalInit(GameContainer.UI.CanvasHint, 0.48f);

            _bar.color = _icon.color = GameContainer.UI.PlayerColors[PlayerId.Satan]; _icon = null;

            Localization.Instance.Subscribe(SetLocalizationText);
            GameContainer.Satan.Subscribe(SetValues, false);

            _indicator.canvasRenderer.SetAlpha(0f);
            GameContainer.Satan.Interactable.Subscribe(IndicatorTurn);
        }

        private void SetLocalizationText(Localization localization)
        {
            StringBuilder sb = new(80);
            sb.AppendLine(GameContainer.UI.PlayerNames[PlayerId.Satan]);
            sb.AppendLine(CONST_UI.SEPARATOR);
            sb.Append(localization.ExtractText(_hintKey));
            _localizedText = sb.ToString();
            
            SetValues(GameContainer.Satan);
        }

        private void SetValues(Satan satan)
        {
            _hintText = string.Format(_localizedText, satan.Curse.ToStr(), satan.MaxCurse.ToStr(), satan.Level.ToStr(), satan.MaxLevel.ToStr());

            _bar.fillAmount = satan.CursePercent;
            _levelTMP.text = satan.Level.ToStr();
        }

        private void IndicatorTurn(bool run)
        {
            _indicatorRun = run;
            if (run) StartCoroutine(IndicatorTurn_Cn());

            // ===== Local =====
            IEnumerator IndicatorTurn_Cn()
            {
                float start = 0f, end = 1f, progress, sign;
                _indicator.fillClockwise = true;

                yield return _waitStartIndicator.Restart();

                _indicator.canvasRenderer.SetAlpha(1f);

                while (_indicatorRun | !_indicator.fillClockwise)
                {
                    progress = 0f; sign = end - start;

                    do
                    {
                        progress += Time.unscaledDeltaTime * _indicatorSpeed;
                        _indicator.fillAmount = start + sign * progress;
                        yield return null;
                    }
                    while (progress < 1f);

                    (start, end) = (end, start);
                    _indicator.fillClockwise = !_indicator.fillClockwise;
                }

                _indicator.canvasRenderer.SetAlpha(0f);
            }
        }

        private void OnDestroy()
        {
            Localization.Instance.Unsubscribe(SetLocalizationText);
        }

#if UNITY_EDITOR

        [StartEditor]
        [SerializeField, Range(.0f, .2f)] private float _iconRatio = 0.1f;
        [SerializeField, Range(.0f, .2f)] private float _indicatorRatio = 0.02f;
        [SerializeField, HideInInspector] private Vector2 _padding = new(16f, -16f);
        [EndEditor] public bool endEditor;

        public void UpdateVisuals_Ed(PlayerColors playerColors, Vector2 padding)
        {
            _bar.color = _icon.color = playerColors[PlayerId.Satan];

            _padding = -padding;
            var rectTransform = (RectTransform)transform;
            rectTransform.anchoredPosition = _padding - rectTransform.sizeDelta * _iconRatio;
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            this.SetChildren(ref _indicator, "Indicator");
            this.SetChildren(ref _bar, "Value");
            this.SetChildren(ref _icon, "Icon");
            this.SetChildren(ref _levelTMP);

            AwaitSet();
        }

        private async void AwaitSet()
        {
            await System.Threading.Tasks.Task.Delay(2);
            if (Application.isPlaying || _icon == null || this == null) return;

            SetRatio(_icon, _iconRatio);
            SetRatio(_indicator, _indicatorRatio);

            var rectTransform = (RectTransform)transform;
            rectTransform.anchoredPosition = _padding - rectTransform.sizeDelta * _iconRatio;

            static void SetRatio(Image image, float ratio)
            {
                var rectTransform = (RectTransform)image.transform;
                float min = -ratio;
                rectTransform.anchorMin = new(min, min);
                float max = 1f + ratio;
                rectTransform.anchorMax = new(max, max);
            }
        }
#endif
    }
}
