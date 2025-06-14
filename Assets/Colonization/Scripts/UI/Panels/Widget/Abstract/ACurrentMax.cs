using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	public abstract class ACurrentMax<TCombination> : AHintWidget where TCombination : ICombination
    {
        protected const string COUNT = "{0,2}<space=0.05em>|<space=0.05em>{1,-2}";

        protected string _textHint;
        protected TCombination _reactiveCurrentMax;

        protected void SetCurrentMax(int current, int max)
        {
            _valueTMP.text = string.Format(COUNT, current, max);
            _text = string.Format(_textHint, current, max);
        }

        protected override void SetLocalizationText(Localization localization)
        {
            _textHint = localization.GetText(_getText.id, _getText.key);
            _reactiveCurrentMax?.Signal();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _reactiveCurrentMax?.Dispose();
        }
    }
}
