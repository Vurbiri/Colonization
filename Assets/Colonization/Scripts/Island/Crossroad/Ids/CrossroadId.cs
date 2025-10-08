namespace Vurbiri.Colonization
{
	public abstract class CrossroadId : IdType<CrossroadId>
	{
		public const int Down = 0;
		public const int Up   = 1;

		[NotId] public const int Max = Up;

        static CrossroadId() => ConstructorRun();
	}
}
