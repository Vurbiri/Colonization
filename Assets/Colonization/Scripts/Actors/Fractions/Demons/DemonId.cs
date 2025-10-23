
namespace Vurbiri.Colonization
{
    public abstract class DemonId : ActorId<DemonId>
	{
		public const int Imp    = 0;
        public const int Bomb   = 1;
        public const int Grunt  = 2;
        public const int Fatty  = 3;
        public const int Boss   = 4;

        static DemonId() => ConstructorRun();
	}
}
