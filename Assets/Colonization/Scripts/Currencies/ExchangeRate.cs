//Assets\Colonization\Scripts\Currencies\ExchangeRate.cs
using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ExchangeRate : IReactive<int, int>, IDisposable
    {
        private readonly Currencies _exchange = new();
        private readonly Unsubscribers _unsubscribers = new();
        private Chance _chance = new(0);
        private int _min, _max;
        
        public ExchangeRate(AbilitiesSet<PlayerAbilityId> abilities)
        {
            _unsubscribers += abilities.GetAbility(PlayerAbilityId.ExchangeRateMin).Subscribe(v => _min = v);
            _unsubscribers += abilities.GetAbility(PlayerAbilityId.ExchangeRateMax).Subscribe(v => _max = v);
            _unsubscribers += abilities.GetAbility(PlayerAbilityId.ExchangeMinChance).Subscribe(v => _chance.Value = v);

            Update();
        }

        public void Update()
        {
            for (int i = 0; i < CurrencyId.CountMain; i++)
                _exchange[i] = _chance.Select(_min, _max);
        }

        public IUnsubscriber Subscribe(Action<int, int> action, bool calling = true) => _exchange.Subscribe(action, calling);
        public void Unsubscribe(Action<int, int> action) => _exchange.Unsubscribe(action);

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _exchange.Dispose();
        }
    }
}
