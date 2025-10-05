using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class ExchangeRate : IReadOnlyList<int>, IReactive<ExchangeRate>, IDisposable
    {
        private const int COUNT = CurrencyId.MainCount;

        private readonly int[] _exchange;
        private readonly VAction<ExchangeRate> _changeValue = new();
        private Subscription _subscription;
        private Chance _chance;
        private int _rate;

        public int this[int index] { [Impl(256)] get => _exchange[index]; }
        public int Count { [Impl(256)] get => COUNT; }

        private ExchangeRate(ReadOnlyAbilities<HumanAbilityId> abilities)
        {
            SubscribeToAbilities(abilities);
            _exchange = new int[COUNT];
            Update();
        }
        private ExchangeRate(int[] data, ReadOnlyAbilities<HumanAbilityId> abilities)
        {
            SubscribeToAbilities(abilities);
            _exchange = data;
        }

        public static ExchangeRate Create(ReadOnlyAbilities<HumanAbilityId> abilities, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.exchange != null)
                return new(loadData.exchange, abilities);
            return new(abilities);
        }

        public Subscription Subscribe(Action<ExchangeRate> action, bool instantGetValue = true) => _changeValue.Add(action, instantGetValue, this);

        public void Update()
        {
            for (int i = 0; i < CurrencyId.MainCount; i++)
                _exchange[i] = _rate - _chance.Select(1);

            _changeValue.Invoke(this);
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        private void SubscribeToAbilities(ReadOnlyAbilities<HumanAbilityId> abilities)
        {
            _subscription += abilities[HumanAbilityId.ExchangeRate].Subscribe(v => _rate = v);
            _subscription += abilities[HumanAbilityId.ExchangeSaleChance].Subscribe(v => _chance.Value += v);
        }

        public IEnumerator<int> GetEnumerator() => new ArrayEnumerator<int>(_exchange, COUNT);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<int>(_exchange, COUNT);
    }
}
