namespace Vurbiri.Colonization
{
	public abstract class CrossroadType : IdType<CrossroadType>
	{
		public const int A = 0;
		public const int Y = 1;

		[NotId] public const int Max = Y;

        static CrossroadType() => ConstructorRun();
	}
}
