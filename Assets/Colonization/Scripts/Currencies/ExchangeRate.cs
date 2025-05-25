using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ExchangeRate : IReactive<CurrenciesLite>, IDisposable
    {
        private readonly CurrenciesLite _exchange;
        private readonly Subscription<CurrenciesLite> _eventChanged = new();
        private Unsubscriptions _unsubscribers = new();
        private Chance _chance;
        private int _rate;

        private ExchangeRate(AbilitiesSet<HumanAbilityId> abilities)
        {
            SubscribeToAbilities(abilities);
            _exchange = new();
            Update();
        }
        private ExchangeRate(int[] data, AbilitiesSet<HumanAbilityId> abilities)
        {
            SubscribeToAbilities(abilities);
            _exchange = new(data);
        }

        public static ExchangeRate Create(AbilitiesSet<HumanAbilityId> abilities, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.exchange != null)
                return new(loadData.exchange, abilities);
            return new(abilities);
        }

        public Unsubscription Subscribe(Action<CurrenciesLite> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _exchange);

        public void Update()
        {
            for (int i = 0; i < CurrencyId.Count; i++)
                _exchange.Set(i, _rate - _chance.Select(1));

            _eventChanged.Invoke(_exchange);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }

        private void SubscribeToAbilities(AbilitiesSet<HumanAbilityId> abilities)
        {
            _unsubscribers += abilities[HumanAbilityId.ExchangeRate].Subscribe(v => _rate = v);
            _unsubscribers += abilities[HumanAbilityId.ExchangeSaleChance].Subscribe(v => _chance.Value += v);
        }
    }
}
