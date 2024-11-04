namespace Vurbiri.Colonization
{
    public class ActorAbilityId : AAbilityd<ActorAbilityId>
    {
        public const int MaxHealth = 0;
        public const int Damage = 1;
        public const int Defense = 2;
        public const int MaxActionPoint = 3;
        public const int ActionPointPerTurn = 4;

        static ActorAbilityId() => RunConstructor();
        private ActorAbilityId() { }
    }
}
