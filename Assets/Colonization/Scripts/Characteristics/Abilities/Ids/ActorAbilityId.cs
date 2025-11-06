namespace Vurbiri.Colonization
{
    public abstract class ActorAbilityId : AbilityId<ActorAbilityId>
    {
        public const int MaxHP       =  0;
        public const int CurrentHP   =  1;
        public const int HPPerTurn   =  2;
        public const int Attack      =  3;
        public const int Defense     =  4;
        public const int MaxAP       =  5;
        public const int CurrentAP   =  6;
        public const int APPerTurn   =  7;
        public const int IsMove      =  8;
        public const int ProfitMain  =  9;
        public const int ProfitAdv   = 10;
        public const int Pierce      = 11;

        [NotId] public const int SHIFT_ABILITY = 10;
        [NotId] public const int MAX_ID_SHIFT_ABILITY = Defense;
    }
}
