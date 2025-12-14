using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
    using static CurrencyId;

    public abstract class ACurrencies
    {
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AllCount;
        }

        public abstract int Amount { get ; }

        public abstract bool IsEmpty { get; }

        public abstract int this[int index] { get; }
        public abstract int this[Id<CurrencyId> id] { get; }

        public static bool operator >=(ACurrencies left, ACurrencies right)
        {
            if (left.Amount < right.Amount)
                return false;

            for (int i = 0; i < MainCount; ++i)
                if (left[i] < right[i])
                    return false;
            return true;
        }
        public static bool operator <=(ACurrencies left, ACurrencies right)
        {
            for (int i = 0; i < MainCount; ++i)
                if (left[i] > right[i])
                    return false;
            return true;
        }

        public static bool operator >(ACurrencies left, ACurrencies right) => !(left <= right);
        public static bool operator <(ACurrencies left, ACurrencies right) => !(left >= right);
    }
}
