using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    sealed public partial class Game : GameEvents, System.IDisposable
    {
        private static Game s_instance;

        private Id<GameModeId> _gameMode;
        private TurnQueue _turnQueue;
        private int _hexId;

        private GameState _gameState;

        private Game() : base()
        {
            _gameMode = GameModeId.Play; //GameModeId.Init
            _turnQueue = new(PlayerId.Player);
            _hexId = -1;
        }

        private Game(Id<GameModeId> gameMode, TurnQueue turnQueue, int hexId) : base()
        {
            _gameMode = gameMode;
            _turnQueue = turnQueue;
            _hexId = hexId;
        }

        public static Game Create(GameState gameState)
        {
            if (s_instance == null)
            {
                if (!gameState.TryGetGame(out s_instance))
                    s_instance = new();

                s_instance._gameState = gameState;
            }
            return s_instance;
        }

        public void Start()
        {
            _gameState.Start();
            Change(_gameMode);
        }

        public void Init()
        {
            _turnQueue.Next();
            if(_turnQueue.currentId != PlayerId.HumansCount)
                Change(GameModeId.Init);
            else
                StartTurn();
        }

        public void Play()
        {
            Change(GameModeId.Play);
        }

        public void EndTurn()
        {
            Change(GameModeId.EndTurn);
        }

        public void StartTurn()
        {
            _turnQueue.Next();

            Change(GameModeId.StartTurn);
        }

        public void WaitRoll()
        {
            Change(GameModeId.WaitRoll);
        }

        public void Roll(int newValue)
        {
            _hexId = newValue;

            Change(GameModeId.Roll);
        }

        public void Profit()
        {
            Change(GameModeId.Profit);
        }

        private void Change(Id<GameModeId> gameMode)
        {
            _changingGameModes[_gameMode = gameMode].Invoke(_turnQueue, _hexId);
            _gameState.Storage.Save(SAVE_KEYS.GAME, this);
        }

        public void Dispose()
        {
            s_instance = null;
        }
    }


}
