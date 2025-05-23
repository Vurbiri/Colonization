//Assets\Colonization\Scripts\GameLoop\GameModeId.cs
namespace Vurbiri.Colonization
{
    public abstract class GameModeId : IdType<GameModeId>
    {
        public const int End       = -1;
        public const int Init      = 0;
        public const int Play      = 1;
        public const int EndTurn   = 2;
        public const int StartTurn = 3;
        public const int WaitRoll  = 4;
        public const int Roll      = 5;
        public const int Profit    = 6;
        

        static GameModeId() => RunConstructor();
    }
}
