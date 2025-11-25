namespace Vurbiri.Colonization
{
	public abstract class WarriorAIStateId : ActorAIStateId<WarriorAIStateId>
	{
		public const int Escape         = 0;
        public const int EscapeSupport  = 1;
        public const int BlockInCombat  = 2;
        public const int Combat         = 3;
        public const int Support        = 4;
        public const int MoveToHelp     = 5;
        public const int Defense        = 6;
        public const int MoveToUnsiege  = 7;
        public const int MoveToAttack   = 8;
        public const int MoveToRaid     = 9;
        public const int MoveToHome     = 10;
        public const int Healing        = 11;
        public const int FindResources  = 12;
        
    }
}
