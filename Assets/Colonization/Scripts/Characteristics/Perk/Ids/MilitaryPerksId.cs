namespace Vurbiri.Colonization
{
    public abstract class MilitaryPerksId : APerkId<MilitaryPerksId>
    {
        [NotId] public const int Type = AbilityTypeId.Military;

        public const int Defense_1        =  0;
        public const int Defense_2        =  1;
        public const int Defense_3        =  2;
        public const int HPPerTurn_1      =  3;

        public const int Attack_1         =  4;
        public const int Attack_2         =  5;
        public const int Attack_3         =  6;
        public const int Attack_4         =  7;
        public const int Pierce_1         =  8;

        public const int IsSolder_1       =  9;
        public const int IsWizard_1       = 10;
        public const int IsWarlock_1      = 11;
        public const int IsKnight_1       = 12;

        public const int MaxWarrior_1     = 13;
        public const int MaxWarrior_2     = 14;

        public const int APPerTurn_1      = 15;
        public const int MaxAP_1          = 16;

        public const int MaxHP_1          = 17;
        public const int MaxHP_2          = 18;
        public const int MaxHP_3          = 19;
        public const int MaxHP_4          = 20;

        public const int ProfitMain_1     = 21;
        public const int ProfitMain_2     = 22;
        public const int IsArtefact_1     = 23;
        public const int ProfitAdv_1      = 24;
        public const int ProfitAdv_2      = 25;
        public const int WarriorProfit_1  = 26;
    }
}
