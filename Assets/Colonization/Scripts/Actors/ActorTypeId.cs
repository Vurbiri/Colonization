namespace Vurbiri.Colonization
{
	public abstract class ActorTypeId : IdType<ActorTypeId>
	{
		public const int Warrior = 0;
        public const int Demon   = 1;

        static ActorTypeId() => ConstructorRun();
	}
}
