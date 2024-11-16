namespace Vurbiri.Colonization.Characteristics
{
    public class ActorAbilityId : AAbilityId<ActorAbilityId>
    {
        public const int MaxHP = 0;
        public const int CurrentHP = 1;
        public const int HPPerTurn = 2;
        public const int Attack = 3;
        public const int Defense = 4;
        public const int MaxAP = 5;
        public const int CurrentAP = 6;
        public const int APPerTurn = 7;
        public const int IsMove = 8;

        [NotId] public const int RATE_ABILITY = 100;
        [NotId] public const int MAX_RATE_ABILITY = Defense;

        static ActorAbilityId() => RunConstructor();
        private ActorAbilityId() { }
    }
}
