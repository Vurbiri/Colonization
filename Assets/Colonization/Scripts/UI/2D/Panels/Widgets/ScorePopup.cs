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

        public void Init(Vector3 offsetPopup)
        {
            base.Init();
            _popup.Init(offsetPopup);

            _valueTMP.text = "0";
            _subscription += GameContainer.Score.Subscribe(SetValue);
        }

        private void SetValue(Score values)
        {
            int score = values[PlayerId.Person];
            if (score != _score)
            {
                var strValue = _score.ToStr();
                _popup.ForceRun(score - _score);
                _score = score;
                _valueTMP.text = strValue;
                _hintText = string.Format(_localizedText, strValue, GameContainer.GameSettings.MaxScore);
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
