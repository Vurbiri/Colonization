namespace Vurbiri.Colonization
{
	public abstract class ActorTypeId : IdType<ActorTypeId>
	{
		public const int Warrior = 0;
        public const int Demon   = 1;

        public static string GetName(Actor actor) => GetName(actor.TypeId, actor.Id);
        public static string GetName(int type, int id)
        {
#if UNITY_EDITOR
            return (type == Warrior ? WarriorId.Names_Ed : DemonId.Names_Ed)[id];
#else
            return (type == Warrior ? "Warrior" : "Demon").Concat("_", id.ToStr());
#endif
        }

    }
}
