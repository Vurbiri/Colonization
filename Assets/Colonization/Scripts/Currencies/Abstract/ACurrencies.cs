//Assets\Colonization\Scripts\Currencies\Abstract\ACurrencies.cs
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public abstract class ACurrencies : IReadOnlyList<int>
    {
        protected const int countMain = CurrencyId.CountMain;
        protected const int countAll = CurrencyId.CountAll;

        public int Count => countAll;
        public abstract int Amount { get ; }

        public abstract int this[int index] { get; }
        public abstract int this[Id<CurrencyId> id] { get; }

        public abstract IEnumerator<int> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool operator >=(ACurrencies left, ACurrencies right)
        {
            if (left.Amount < right.Amount)
                return false;

            for (int i = 0; i < countMain; i++)
                if (left[i] < right[i])
                    return false;
            return true;
        }
        public static bool operator <=(ACurrencies left, ACurrencies right)
        {
            for (int i = 0; i < countMain; i++)
                if (left[i] > right[i])
                    return false;
            return true;
        }

        //public static bool operator >(ACurrencies left, ACurrencies right) => !(left <= right);
        //public static bool operator <(ACurrencies left, ACurrencies right) => !(left >= right);
    }
}
