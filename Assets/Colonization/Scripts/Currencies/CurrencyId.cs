namespace Vurbiri.Colonization
{
    public abstract class CurrencyId : IdType<CurrencyId>
    {
        public const int Gold  = 0;
        public const int Food  = 1;
        public const int Wood  = 2;
        public const int Ore   = 3;
        public const int Mana  = 4;
        public const int Blood = 5;

        [NotId] public const int MainCount = 5;
        [NotId] public const int AllCount = 6;
    }
}
