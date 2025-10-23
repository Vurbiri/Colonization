using TMPro;

namespace Vurbiri.Colonization.UI
{
    public class APBar : System.IDisposable
    {
		private const char CHAR = '+';

        private readonly TextMeshPro _maxValueTMP;
        private readonly TextMeshPro _currentValueTMP;
        private readonly Subscription _subscription;

        public APBar(TextMeshPro maxValue, TextMeshPro currentValue, ReadOnlyAbilities<ActorAbilityId> abilities)
        {
            _maxValueTMP = maxValue;
            _currentValueTMP = currentValue;

            _subscription  = abilities[ActorAbilityId.MaxAP].Subscribe(SetMaxValue);
            _subscription += abilities[ActorAbilityId.CurrentAP].Subscribe(SetCurrentValue);
        }

        public void Dispose() => _subscription.Dispose();

        private void SetMaxValue(int value) => _maxValueTMP.text = new(CHAR, value);
        private void SetCurrentValue(int value) => _currentValueTMP.text = new(CHAR, value);
	}
}
