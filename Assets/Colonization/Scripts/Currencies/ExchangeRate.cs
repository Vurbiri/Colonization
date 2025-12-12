using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class ExchangeRate : IReadOnlyList<int>, IReactive<ExchangeRate>
    {
        private const int COUNT = CurrencyId.MainCount;

        private readonly int[] _exchange;
        private readonly ReactiveVersion<ExchangeRate> _version = new();
        private Chance _chance;
        private int _rate;

        public int this[int index] { [Impl(256)] get => _exchange[index]; }
        public int Count { [Impl(256)] get => COUNT; }

        private ExchangeRate(ReadOnlyAbilities<HumanAbilityId> abilities, PerkTree perks) : this(new int[COUNT], abilities, perks) => Update();
        private ExchangeRate(int[] data, ReadOnlyAbilities<HumanAbilityId> abilities, PerkTree perks)
        {
            _exchange = data;

            abilities[HumanAbilityId.ExchangeRate].Subscribe(OnExchangeRate);
            abilities[HumanAbilityId.ExchangeSaleChance].Subscribe(OnExchangeSaleChance);
 
            perks.Subscribe(LearnedPerk, false);
        }

        public static ExchangeRate Create(ReadOnlyAbilities<HumanAbilityId> abilities, PerkTree perks, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.exchange != null)
                return new(loadData.exchange, abilities, perks);
            return new(abilities, perks);
        }

        public Subscription Subscribe(Action<ExchangeRate> action, bool instantGetValue = true)
        {
            if (instantGetValue) action(this);
            return _version.Add(action);
        }

        public void Update()
        {
            for (int i = 0; i < CurrencyId.MainCount; ++i)
                _exchange[i] = _rate - _chance.Select(1);

            _version.Next(this);
        }

        public IEnumerator<int> GetEnumerator() => new ArrayEnumerator<int>(_exchange, COUNT, _version);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<int>(_exchange, COUNT, _version);

        private void OnExchangeRate(int value) => _rate = value;
        private void OnExchangeSaleChance(int value) => _chance.Value = value;
        private void LearnedPerk(Perk perk)
        {
            if (perk.Type == EconomicPerksId.Type & (perk.TargetAbility >= HumanAbilityId.ExchangeSaleChance | perk.TargetAbility == HumanAbilityId.ExchangeRate))
                Update_Cn().Start();

            //Local
            IEnumerator Update_Cn()
            {
                yield return null;
                Update();
            }
        }
    }
}
