using System;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ExchangeRate : IReactive<ACurrencies>, IDisposable
    {
        private readonly CurrenciesLite _exchange;
        private readonly Subscription<ACurrencies> _changeValue = new();
        private Unsubscription _unsubscribers;
        private Chance _chance;
        private int _rate;

        public int this[int index] => _exchange[index];
        public int this[Id<PlayerId> id] => _exchange[id.Value];

        private ExchangeRate(ReadOnlyAbilities<HumanAbilityId> abilities)
        {
            SubscribeToAbilities(abilities);
            _exchange = new();
            Update();
        }
        private ExchangeRate(int[] data, ReadOnlyAbilities<HumanAbilityId> abilities)
        {
            SubscribeToAbilities(abilities);
            _exchange = new(data);
        }

        public static ExchangeRate Create(ReadOnlyAbilities<HumanAbilityId> abilities, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.exchange != null)
                return new(loadData.exchange, abilities);
            return new(abilities);
        }

        public Unsubscription Subscribe(Action<ACurrencies> action, bool instantGetValue = true) => _changeValue.Add(action, instantGetValue, _exchange);

        public void Update()
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
                _exchange.SetMain(i, _rate - _chance.Select(1));

            _changeValue.Invoke(_exchange);
        }

        public void Dispose()
        {
            _unsubscribers?.Dispose();
        }

        private void SubscribeToAbilities(ReadOnlyAbilities<HumanAbilityId> abilities)
        {
            _unsubscribers += abilities[HumanAbilityId.ExchangeRate].Subscribe(v => _rate = v);
            _unsubscribers += abilities[HumanAbilityId.ExchangeSaleChance].Subscribe(v => _chance.Value += v);
        }
    }
}
