//Assets\Colonization\Scripts\Balance\Balance.cs
using System;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public class Balance : IReactive<Win>
	{
        private readonly RInt _value;
        private readonly BalanceSettings _settings;
        private readonly Subscriber<Win> _subscriber = new();
        
        public Balance(BalanceSettings settings, Players players)
        {
            _value = new();
            _settings = settings;
            for (int i = 0; i < PlayerId.PlayersCount; i++)
            {
                players[i].Shrines.Subscribe(OnShrineBuild, false);
            }

            players.Satan.SetBalance(_value);
        }

        public Unsubscriber Subscribe(Action<Win> action, bool calling = true) => _subscriber.Add(action);

        public void Add(int value)
        {
            _value.Add(value);

            if (_value <= _settings.min)
                _subscriber.Invoke(Win.Satan);
            if (_value >= _settings.max)
                _subscriber.Invoke(Win.Human);
        }

        private void OnShrineBuild(int index, Crossroad crossroad, TypeEvent type)
        {
            if (type == TypeEvent.Add)
                Add(_settings.perShrine);
        }
    }
}
