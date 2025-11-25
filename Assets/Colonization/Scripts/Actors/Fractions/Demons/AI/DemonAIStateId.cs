namespace Vurbiri.Colonization
{
	public abstract class DemonAIStateId : ActorAIStateId<DemonAIStateId>
    {
        public const int ExitFromGate = 0;
        public const int UseSpecSkill = 1;
        public const int Combat       = 2;
        public const int MoveToHelp   = 3;
        public const int MoveToRaid   = 4;
        public const int MoveToEnemy  = 5;
        public const int Healing      = 6;
        public const int Defense      = 7;
        public const int MoveToAttack = 8;
        public const int FreeFinding  = 9;
    }
}
