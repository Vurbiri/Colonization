namespace Vurbiri.Colonization
{
    public abstract class GameModeId : IdType<GameModeId>
    {
        public const int Landing      = 0;
        public const int EndLanding   = 1;
        public const int Play      = 2;
        public const int EndTurn   = 3;
        public const int StartTurn = 4;
        public const int WaitRoll  = 5;
        public const int Roll      = 6;
        public const int Profit    = 7;
        public const int End       = 8;

        static GameModeId() => RunConstructor();
    }
}
