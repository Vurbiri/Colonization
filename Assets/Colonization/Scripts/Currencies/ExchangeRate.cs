//Assets\Colonization\Scripts\Currencies\ExchangeRate.cs
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class ExchangeRate
    {
        private readonly CurrenciesLite _exchange = new();
        private readonly Unsubscribers _unsubscribers = new();
        private Chance _chance;
        private int _min, _max;
        
        public ExchangeRate(IReadOnlyAbilities<HumanAbilityId> abilities)
        {
            _unsubscribers += abilities[HumanAbilityId.ExchangeRateMin].Subscribe(v => _min = v);
            _unsubscribers += abilities[HumanAbilityId.ExchangeRateMax].Subscribe(v => _max = v);
            _unsubscribers += abilities[HumanAbilityId.ExchangeMinChance].Subscribe(v => _chance.Value = v);

            Update();
        }

        public void Update()
        {
            for (int i = 0; i < CurrencyId.CountMain; i++)
                _exchange.Set(i, _chance.Select(_min, _max));
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();

        }
    }
}
