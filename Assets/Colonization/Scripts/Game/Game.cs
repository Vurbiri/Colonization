using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    sealed public partial class Game : GameEvents, System.IDisposable
    {
        private static Game s_instance;

        private Id<GameModeId> _gameMode;
        private TurnQueue _turnQueue;
        private int _hexId;

        private GameplayStorage _storage;

        private Game() : this(GameModeId.Play/*Init*/, new(PlayerId.Player), -1) { }
        private Game(Id<GameModeId> gameMode, TurnQueue turnQueue, int hexId) : base()
        {
            _gameMode = gameMode;
            _turnQueue = turnQueue;
            _hexId = hexId;
        }

        public static Game Create(GameplayStorage storage)
        {
            if (s_instance == null)
            {
                if (!storage.TryGetGame(out s_instance))
                    s_instance = new();

                s_instance._storage = storage;
            }
            return s_instance;
        }

        public void Start()
        {
            Change(_gameMode);
        }

        public void Init()
        {
            _turnQueue.Next();

            Change(GameModeId.Init);
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

        public void End(Winner winner)
        {
            Change(GameModeId.End);
        }

        private void Change(Id<GameModeId> gameMode)
        {
            _gameMode = gameMode;
            _changingGameModes[gameMode].Invoke(_turnQueue, _hexId);
            _storage.SaveGame(this);
        }

        public void Dispose()
        {
            s_instance = null;
        }
    }


}
