using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
	sealed public class ScorePopup : AHintWidget
    {
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        private string _localizedText;
        private int _score;

        public void Init(Direction2 offsetPopup)
        {
            base.Init();
            _popup.Init(offsetPopup);

            _valueTMP.text = "0";
            _unsubscribers += GameContainer.Score.Subscribe(SetValue);
        }

        private void SetValue(int[] values)
        {
            int delta = values[PlayerId.Person] - _score;
            if (delta > 0)
            {
                _score = values[PlayerId.Person];
                _valueTMP.text = _score.ToString();
                _popup.ForceRun(delta);
                _hintText = string.Format(_localizedText, _score, GameContainer.GameSettings.MaxScore);
            }
        }

        protected override void SetLocalizationText(Localization localization)
        {
            _localizedText = localization.GetText(_getText.id, _getText.key);
            _hintText = string.Format(_localizedText, _score, GameContainer.GameSettings.MaxScore);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            this.SetChildren(ref _popup);
        }
#endif
    }
}
