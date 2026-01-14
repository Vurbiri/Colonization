namespace Vurbiri.Colonization
{
	public abstract class PlayerId : IdType<PlayerId>
	{
		public const int Person = 0;
		public const int AI_01  = 1;
		public const int AI_02  = 2;
		public const int Satan  = 3;

		[NotId] public const int HumansCount = 3;
		[NotId] public const int AICount = 2;
	}
}
