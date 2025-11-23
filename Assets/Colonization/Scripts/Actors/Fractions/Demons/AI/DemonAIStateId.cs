namespace Vurbiri.Colonization
{
	public abstract class DemonAIStateId : ActorAIStateId<DemonAIStateId>
    {
        public const int ExitFromGate  = 0;
        public const int UseSpecSkill  = 1;
        public const int EscapeSupport = 2;
        public const int Combat        = 3;
        public const int Support       = 4;
        public const int MoveToHelp    = 5;
        public const int MoveToEnemy   = 6;
        public const int MoveToRaid    = 7;
        public const int MoveToAttack  = 8;
        public const int Defense       = 9;
    }
}
