using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class SatanPanel : AHintElement
    {
        [SerializeField] protected FileIdAndKey _hintKey;
        [Space]
        [SerializeField] private Image _bar;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _levelTMP;

        private string _localizedText;

        public void Init()
        {
            base.InternalInit(GameContainer.UI.CanvasHint, 0.48f);

            _bar.color = _icon.color = GameContainer.UI.PlayerColors[PlayerId.Satan]; _icon = null;

            Localization.Instance.Subscribe(SetLocalizationText);
            GameContainer.Players.Satan.Subscribe(SetValues, false);
        }

        private void SetLocalizationText(Localization localization)
        {
            StringBuilder sb = new(80);
            sb.AppendLine(GameContainer.UI.PlayerNames[PlayerId.Satan]);
            sb.AppendLine(CONST_UI.SEPARATOR);
            sb.Append(localization.GetText(_hintKey));
            _localizedText = sb.ToString();
            
            SetValues(GameContainer.Players.Satan);
        }

        private void SetValues(Satan satan)
        {
            _hintText = string.Format(_localizedText, satan.Curse, satan.MaxCurse, satan.Level, satan.MaxLevel);

            _bar.fillAmount = satan.CursePercent;
            _levelTMP.text = CONST.NUMBERS_STR[satan.Level];
        }

        private void OnDestroy()
        {
            Localization.Instance.Unsubscribe(SetLocalizationText);
        }

#if UNITY_EDITOR

        [StartEditor]
        [SerializeField, Range(.0f, .2f)] private float _iconRatio = 0.1f;
        [SerializeField, HideInInspector] private Vector2 _padding = new(16f, -16f);
        [EndEditor] public bool endEditor;

        public void UpdateVisuals_Ed(PlayerColors playerColors, Vector2 padding)
        {
            _bar.color = _icon.color = playerColors[PlayerId.Satan];

            _padding = new(padding.x, -padding.y);
            ((RectTransform)transform).anchoredPosition = (1f + 2f * _iconRatio) * _padding;
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            this.SetChildren(ref _bar, "Bar");
            this.SetChildren(ref _icon, "Icon");
            this.SetChildren(ref _levelTMP);

            AwaitSet();
        }

        private async void AwaitSet()
        {
            await System.Threading.Tasks.Task.Delay(2);
            if (Application.isPlaying) return;

            var rectTransform = (RectTransform)_icon.transform;
            float min = -_iconRatio;
            rectTransform.anchorMin = new(min, min);
            float max = 1f + _iconRatio;
            rectTransform.anchorMax = new(max, max);

            rectTransform = (RectTransform)transform;
            rectTransform.anchoredPosition = (1f + 2f * _iconRatio) * _padding;
        }
#endif
    }
}
