namespace Vurbiri.Colonization
{
    public class ActorAbilityId : AAbilityId<ActorAbilityId>
    {
        public const int MaxHP = 0;
        public const int RegenerationHP = 1;
        public const int Attack = 2;
        public const int Defense = 3;
        public const int MaxActionPoint = 4;
        public const int ActionPointPerTurn = 5;

        static ActorAbilityId() => RunConstructor();
        private ActorAbilityId() { }
    }
}
