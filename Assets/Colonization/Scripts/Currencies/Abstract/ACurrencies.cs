using UnityEngine;

namespace Vurbiri.Colonization
{

    public abstract class ACurrencies
    {
        [SerializeField] protected int _amount = 0;

        protected const int countMain = CurrencyId.CountMain;
        protected const int countAll = CurrencyId.CountAll;

        public virtual int Amount { get => _amount; protected set => _amount = value; }

        public abstract int this[int index] { get; }
        public abstract int this[Id<CurrencyId> id] { get; }
                
        public static bool operator >=(ACurrencies left, ACurrencies right)
        {
            if (left._amount < right._amount)
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
