//Assets\Colonization\Scripts\GameLoop\GameModeId.cs
namespace Vurbiri.Colonization
{
    public abstract class GameModeId : IdType<GameModeId>
    {
        public const int End       = -1;
        public const int Play      = 0;
        public const int EndTurn   = 1;
        public const int StartTurn = 2;
        public const int WaitRoll  = 3;
        public const int Roll      = 4;
        public const int Profit    = 5;
        public const int Start     = 6;

        static GameModeId() => RunConstructor();
    }
}
