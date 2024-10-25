namespace Vurbiri.Colonization
{
    public class ActorStateId : AStateId<ActorStateId>
    {
        public const int MaxHealth = 0;
        public const int MinDamage = 1;
        public const int MaxDamage = 2;
        public const int MinDefense = 3;
        public const int MaxDefense = 4;
        public const int MoveRange = 5;
        public const int AttackRange = 6;
        public const int MaxActionPoint = 7;

        static ActorStateId() => RunConstructor();
        private ActorStateId() { }
    }
}
