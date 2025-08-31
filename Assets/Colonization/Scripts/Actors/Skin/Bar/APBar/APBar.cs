using TMPro;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors.UI
{
    public class APBar : System.IDisposable
    {
		private const char CHAR = '+';

        private readonly TextMeshPro _maxValueTMP;
        private readonly TextMeshPro _currentValueTMP;
        private readonly Unsubscriptions _unsubscribers;

        public APBar(TextMeshPro maxValue, TextMeshPro currentValue, ReadOnlyAbilities<ActorAbilityId> abilities)
        {
            _maxValueTMP = maxValue;
            _currentValueTMP = currentValue;

            _unsubscribers += abilities[ActorAbilityId.MaxAP].Subscribe(SetMaxValue);
            _unsubscribers += abilities[ActorAbilityId.CurrentAP].Subscribe(SetCurrentValue);
        }

        public void Dispose() => _unsubscribers.Unsubscribe();

        private void SetMaxValue(int value) => _maxValueTMP.text = new(CHAR, value);
        private void SetCurrentValue(int value) => _currentValueTMP.text = new(CHAR, value);
	}
}
