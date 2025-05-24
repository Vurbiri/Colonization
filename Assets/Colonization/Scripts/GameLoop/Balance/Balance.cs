using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
	public class Balance
	{
        private int _value;

        private readonly BalanceSettings _settings;
        private readonly Subscription<Win> _eventWin = new();


        public void AddBalance(int value)
        {
            _value += value;

            if (_value <= _settings.minBalance)
                _eventWin.Invoke(Win.Satan);
            if (_value >= _settings.maxBalance)
                _eventWin.Invoke(Win.Human);
        }
    }
}
