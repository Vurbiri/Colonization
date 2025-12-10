using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
	sealed public class ScorePopup : AHintWidget
    {
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        private string _localizedText;
        private int _score = int.MaxValue;

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
                int delta = score - _score;
                var strValue = score.ToStr();

                _score = score;
                _valueTMP.text = strValue;
                _hintText = string.Format(_localizedText, strValue, GameContainer.GameSettings.MaxScore);

                if (delta > 0) _popup.ForceRun(delta);
            }
        }

        protected override void SetLocalizationText(Localization localization)
        {
            _localizedText = localization.GetText(_getText.id, _getText.key, _extract);
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
