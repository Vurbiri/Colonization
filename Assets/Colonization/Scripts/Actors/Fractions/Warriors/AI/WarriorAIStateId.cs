namespace Vurbiri.Colonization
{
	public abstract class WarriorAIStateId : ActorAIStateId<WarriorAIStateId>
	{
		public const int Escape         = 0;
        public const int Combat         = 1;
        public const int Support        = 2;
        public const int MoveToHelp     = 3;
        public const int Defense        = 4;
        public const int MoveToUnsiege  = 5;
        public const int MoveToAttack   = 6;
        public const int MoveToRaid     = 7;
        public const int MoveToHome     = 8;
        public const int FindResources  = 9;
    }
}
