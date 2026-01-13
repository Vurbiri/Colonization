using System;
using System.Text;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class ReadOnlyCurrencies : IReactive<ReadOnlyCurrencies>
    {
        protected readonly Currency[] _values = new Currency[CurrencyId.Count];
        protected readonly Blood _blood;
        protected ReactiveInt _amount = new(0);
        protected Ability _maxAmount;
        protected readonly VAction<ReadOnlyCurrencies> _changeEvent = new();

        public int Amount { [Impl(256)] get => _amount; }
        public Reactive<int> CurrentAmount { [Impl(256)] get => _amount; }
        public Reactive<int> MaxAmount { [Impl(256)] get => _maxAmount; }
        public int PercentAmount { [Impl(256)] get => (100 * _amount) / _maxAmount; }
        public bool More { [Impl(256)] get => _amount > _maxAmount; }

        public Blood Blood { [Impl(256)] get => _blood; }

        public bool IsOverResources { [Impl(256)] get => _maxAmount.Value < _amount.Value; }
        public bool IsEmpty { [Impl(256)] get => _amount == 0 & _blood.Value == 0; }

        public int this[int index] { [Impl(256)] get => _values[index].Value; }
        public int this[Id<CurrencyId> id] { [Impl(256)] get => _values[id.Value].Value; }

        #region Min/Max
        public int MinIndex
        {
            get
            {
                int minId = 0;
                for (int i = 1; i < CurrencyId.Count; ++i)
                    if (_values[i] < _values[minId])
                        minId = i;
                return minId;
            }
        }
        public int MaxIndex
        {
            get
            {
                int maxId = 0;
                for (int i = 1; i < CurrencyId.Count; ++i)
                    if (_values[i] > _values[maxId])
                        maxId = i;
                return maxId;
            }
        }
        #endregion

        #region Constructions
        protected ReadOnlyCurrencies(StartCurrencies start, Ability maxMainValue, Ability maxBloodValue)
        {
            _maxAmount = maxMainValue;

            for (int i = 0; i < CurrencyId.Count; ++i)
                _values[i] = start[i];
            _amount.SilentValue = start.Amount;

            _blood = new Blood(start.BloodValue, maxBloodValue, RedirectBlood);
        }
        #endregion

        [Impl(256)] public Subscription Subscribe(Action<ReadOnlyCurrencies> action, bool instantGetValue = true) => _changeEvent.Add(action, this, instantGetValue);

        [Impl(256)] public Currency Get(Id<CurrencyId> id) => _values[id.Value];
        [Impl(256)] public ACurrency Get(Id<ProfitId> id) => id == ProfitId.Blood ? _blood : _values[id.Value];

        [Impl(256)] public int PercentAmountExCurrency(Id<CurrencyId> currencyId)
        {
            int currency = _values[currencyId].Value;
            return 100 * (_amount - currency) / (_maxAmount - currency);
        }

        public int OverCount(ReadOnlyLiteCurrencies values, out int lastIndex)
        {
            int count = 0; lastIndex = -1;
            for (int i = 0; i < CurrencyId.Count; ++i)
            {
                if (values[i] > _values[i])
                {
                    ++count; lastIndex = i;
                }
            }
            return count;
        }
        public int Deficit(ReadOnlyLiteCurrencies values)
        {
            int delta = 0;
            for (int i = 0, cost; i < CurrencyId.Count; ++i)
            {
                cost = _values[i] - values[i];
                if (cost < 0)
                    delta += cost;
            }
            return delta;
        }

        public void ToStringBuilder(StringBuilder sb)
        {
            for (int i = 0; i < CurrencyId.Count; ++i)
                sb.AppendFormat(TAG.CURRENCY, i, _values[i].ToString());

            sb.Append(" ");
            sb.Append(_amount.Value.ToStr()); sb.Append("/"); sb.Append(_maxAmount.Value.ToStr());
        }

        sealed public override string ToString()
        {
            StringBuilder sb = new(32); 
            sb.Append("[");
            for (int i = 0; i < CurrencyId.Count - 1; ++i)
            {
                sb.Append(_values[i].ToString()); sb.Append(", ");
            }
            sb.Append(_values[CurrencyId.Count - 1].ToString());
            sb.Append("] ["); sb.Append(_blood.ToString()); sb.Append("|"); sb.Append(_blood.Max.Value.ToStr());  sb.Append("]  ");
            sb.Append(_amount.Value.ToStr()); sb.Append("|"); sb.Append(_maxAmount.Value.ToStr());
            return sb.ToString();
        }

        private void RedirectBlood(int value) => _changeEvent.Invoke(this);
    }
}
