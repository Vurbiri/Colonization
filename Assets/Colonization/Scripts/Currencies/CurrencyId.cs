namespace Vurbiri.Colonization
{
    public class CurrencyId : AIdType<CurrencyId>
    {
        public const int Gold = 0;
        public const int Food = 1;
        public const int Mana = 2;
        public const int Ore  = 3;
        public const int Wood = 4;
        public const int Blood = 5;

        public static int CountMain => 5;

        static CurrencyId()
        {
            RunConstructor();
        }
        private CurrencyId() { }
    }
}
