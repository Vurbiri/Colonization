namespace Vurbiri.Colonization
{

    public abstract class ACurrencies
    {
        protected const int countMain = CurrencyId.CountMain;
        protected const int countAll = CurrencyId.CountAll;

        public abstract int Amount { get ; }

        public abstract int this[int index] { get; }
        public abstract int this[Id<CurrencyId> id] { get; }
                
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
