//Assets\Colonization\Scripts\Currencies\ExchangeRate.cs
using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ExchangeRate : IReactive<CurrenciesLite>
    {
        private readonly CurrenciesLite _exchange;
        private readonly Subscriber<CurrenciesLite> _subscriber = new();
        private Unsubscribers _unsubscribers = new();
        private Chance _chance;
        private int _rate;

        private ExchangeRate(AbilitiesSet<HumanAbilityId> abilities)
        {
            Subscribe(abilities);
            _exchange = new();
            Update();
        }
        private ExchangeRate(int[] data, AbilitiesSet<HumanAbilityId> abilities) : this(abilities)
        {
            Subscribe(abilities);
            _exchange = new(data);
        }

        public static ExchangeRate Create(AbilitiesSet<HumanAbilityId> abilities, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.exchange != null)
                return new(loadData.exchange, abilities);
            return new(abilities);
        }

        public Unsubscriber Subscribe(Action<CurrenciesLite> action, bool calling = true) => _subscriber.Add(action, calling, _exchange);

        public void Update()
        {
            for (int i = 0; i < CurrencyId.Count; i++)
                _exchange.Set(i, _rate - _chance.Select(1));
        }

        public void Dispose()
        {
            _subscriber.Dispose();
            _unsubscribers.Unsubscribe();
        }

        private void Subscribe(AbilitiesSet<HumanAbilityId> abilities)
        {
            _unsubscribers += abilities[HumanAbilityId.ExchangeRate].Subscribe(v => _rate = v);
            _unsubscribers += abilities[HumanAbilityId.ExchangeSaleChance].Subscribe(v => _chance.Value += v);
        }
    }
}
