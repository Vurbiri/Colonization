namespace Vurbiri.Colonization
{
	public abstract class ActorTypeId : IdType<ActorTypeId>
	{
		public const int Warrior = 0;
        public const int Demon   = 1;

#if UNITY_EDITOR
        public static string GetName(int type, int id) => (type == Warrior ? WarriorId.Names_Ed : DemonId.Names_Ed)[id];
#endif
    }
}
