using UnityEngine;
using UnityEngine.UI;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ChaosPanel : AHintElement
    {
        [SerializeField] private FileIdAndKey _hintKey;
        [Space]
        [SerializeField] private Gradient _color;
        [Space]
        [SerializeField] private Image _bar;

        private string _localizedText;
        private int _max;

        public void Init()
        {
            base.InternalInit(GameContainer.UI.CanvasHint, 0.3f);

            _max = GameContainer.Chaos.Max;

            Localization.Instance.Subscribe(SetLocalizationText);
            GameContainer.Chaos.Subscribe(SetValue, false);
        }

        private void SetLocalizationText(Localization localization)
        {
            _localizedText = localization.GetText(_hintKey);

            SetValue(GameContainer.Chaos.Value);
        }

        private void SetValue(int value)
        {
            _hintText = string.Format(_localizedText, value, _max);
            Log.Info($"Chaos: {value}");

            float progress = Mathf.Clamp01((float) value / _max);

            _bar.fillAmount = progress;
            _bar.canvasRenderer.SetColor(_color.Evaluate(progress));
        }

        private void OnDestroy()
        {
            Localization.Instance.Unsubscribe(SetLocalizationText);
        }

#if UNITY_EDITOR

        public void UpdateVisuals_Ed(Vector2 padding)
        {
            var rectTransform = (RectTransform)transform;
            rectTransform.anchoredPosition = -padding;
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            this.SetChildren(ref _bar, "Bar");
        }
#endif
    }
}
