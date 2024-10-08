using System.Text;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class ACurrencies
    {
        [SerializeField] protected int _amount = 0;

        protected readonly int _countMain = CurrencyId.CountMain;
        protected readonly int _countAll = CurrencyId.CountAll;

        public int CountMain => _countMain;
        public int CountAll => _countAll;
        public virtual int Amount { get => _amount; protected set => _amount = value; }

        public abstract int this[int index] { get; }
        public abstract int this[Id<CurrencyId> id] { get; }
                
        public static bool operator >=(ACurrencies left, ACurrencies right)
        {
            if (left._amount < right._amount)
                return false;

            for (int i = 0; i < left._countMain; i++)
                if (left[i] < right[i])
                    return false;
            return true;
        }
        public static bool operator <=(ACurrencies left, ACurrencies right)
        {
            for (int i = 0; i < left._countMain; i++)
                if (left[i] > right[i])
                    return false;
            return true;
        }
        //public static bool operator >(ACurrencies left, ACurrencies right) => !(left <= right);
        //public static bool operator <(ACurrencies left, ACurrencies right) => !(left >= right);

        public override string ToString()
        {
            StringBuilder sb = new(85);
            for (int i = 0; i < _countAll; i++)
                sb.Append($"{i} = {this[i]} | ");
            sb.Append($" Amount: {_amount}");
            return sb.ToString();
        }

    }
}
