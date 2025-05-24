using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    public abstract class ACurrencies : IReadOnlyList<int>
    {
        public int Count => CountAll;
        public abstract int Amount { get ; }

        public abstract int this[int index] { get; }
        public abstract int this[Id<CurrencyId> id] { get; }

        public abstract IEnumerator<int> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool operator >=(ACurrencies left, ACurrencies right)
        {
            if (left.Amount < right.Amount)
                return false;

            for (int i = 0; i < CountMain; i++)
                if (left[i] < right[i])
                    return false;
            return true;
        }
        public static bool operator <=(ACurrencies left, ACurrencies right)
        {
            for (int i = 0; i < CountMain; i++)
                if (left[i] > right[i])
                    return false;
            return true;
        }

        public static bool operator >(ACurrencies left, ACurrencies right) => !(left <= right);
        public static bool operator <(ACurrencies left, ACurrencies right) => !(left >= right);
    }
}
