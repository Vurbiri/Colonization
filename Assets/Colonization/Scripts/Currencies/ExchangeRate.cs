//Assets\Colonization\Scripts\Currencies\ExchangeRate.cs
using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ExchangeRate : IReactive<int, int>
    {
        private readonly Currencies _exchange = new();
        private readonly Unsubscribers _unsubscribers = new();
        private Chance _chance;
        private int _min, _max;
        
        public ExchangeRate(IReadOnlyAbilities<PlayerAbilityId> abilities)
        {
            _unsubscribers += abilities[PlayerAbilityId.ExchangeRateMin].Subscribe(v => _min = v);
            _unsubscribers += abilities[PlayerAbilityId.ExchangeRateMax].Subscribe(v => _max = v);
            _unsubscribers += abilities[PlayerAbilityId.ExchangeMinChance].Subscribe(v => _chance.Value = v);

            Update();
        }

        public void Update()
        {
            for (int i = 0; i < CurrencyId.CountMain; i++)
                _exchange[i] = _chance.Select(_min, _max);
        }

        public Unsubscriber Subscribe(Action<int, int> action, bool calling = true) => _exchange.Subscribe(action, calling);

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _exchange.Dispose();
        }
    }
}
