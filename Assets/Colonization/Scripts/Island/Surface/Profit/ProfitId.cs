namespace Vurbiri.Colonization
{
	public abstract class ProfitId : IdType<ProfitId>
	{
        public const int Gold  = 0;
        public const int Food  = 1;
        public const int Wood  = 2;
        public const int Ore   = 3;
        public const int Mana  = 4;
        public const int Blood = 5;
    }
}
