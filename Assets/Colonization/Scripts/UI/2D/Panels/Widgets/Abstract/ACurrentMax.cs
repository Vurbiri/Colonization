using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	public abstract class ACurrentMax<TCombination> : AHintWidget where TCombination : ICombination
    {
        protected const string COUNT = "{0,2}|{1,-2}^";

        protected string _localizedText;
        protected TCombination _reactiveCurrentMax;

        protected void SetCurrentMax(int current, int max)
        {
            _valueTMP.text = string.Format(COUNT, CONST.NUMBERS_STR[current], CONST.NUMBERS_STR[max]);
            _hintText = string.Format(_localizedText, CONST.NUMBERS_STR[current], CONST.NUMBERS_STR[max]);
        }

        protected override void SetLocalizationText(Localization localization)
        {
            _localizedText = localization.GetText(_getText.id, _getText.key);
            _reactiveCurrentMax?.Signal();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _reactiveCurrentMax?.Dispose();
        }
    }
}
