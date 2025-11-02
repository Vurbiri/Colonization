using System.Collections;
using Vurbiri.Colonization.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public partial class GameLoop : GameEvents
    {
        private GameStorage _storage;

        private Id<GameModeId> _gameMode;
        private TurnQueue _turnQueue;
        private int _hexId;
         
        public Id<GameModeId> GameMode { [Impl(256)] get => _gameMode; }
        
        private GameLoop() : this(GameModeId.Landing, new(PlayerId.Person), -1) { }
        private GameLoop(Id<GameModeId> gameMode, TurnQueue turnQueue, int hexId) : base()
        {
            _gameMode = gameMode;
            _turnQueue = turnQueue;
            _hexId = hexId;
        }

        public static GameLoop Create(GameStorage storage)
        {
            if (!storage.TryGetGame(out GameLoop instance))
                instance = new();
            instance._storage = storage;

            return instance;
        }

        [Impl(256)] public void Start()
        {
            _turnQueue.SetPerson();
            SetGameMode(_gameMode);
        }

        [Impl(256)] public void Landing()
        {
            _turnQueue.Next();
            SetGameMode(GameModeId.Landing, false);
        }
        [Impl(256)] public void EndLanding() => SetGameMode(GameModeId.EndLanding, false);

        [Impl(256)] public void EndTurn() => SetGameMode(GameModeId.EndTurn);
        [Impl(256)] public void StartTurn()
        {
            _turnQueue.Next();
            SetGameMode(GameModeId.StartTurn);
        }

        [Impl(256)] public void WaitRoll() => SetGameMode(GameModeId.WaitRoll);
        [Impl(256)] public void Roll(int newValue)
        {
            _hexId = newValue;
            SetGameMode(GameModeId.Roll);
        }

        [Impl(256)] public void Profit() => SetGameMode(GameModeId.Profit);

        [Impl(256)] public void Play() => SetGameMode(GameModeId.Play);

        public void End_Cn(Winner winner)
        {
            _gameMode = GameModeId.End;
            _changingGameModes[GameModeId.End].Invoke(_turnQueue, _hexId);
            _storage.SaveGame(this);
        }

        [Impl(256)] public bool IsPersonTurn(Id<PlayerId> id) => _gameMode == GameModeId.Play & _turnQueue.currentId == id;

        [Impl(256)] private void SetGameMode(Id<GameModeId> gameMode, bool save = true) => GameContainer.Shared.StartCoroutine(SetGameMode_Cn(gameMode, save));
        private IEnumerator SetGameMode_Cn(Id<GameModeId> gameMode, bool save)
        {
            yield return null;

            _gameMode = gameMode;
            _changingGameModes[gameMode].Invoke(_turnQueue, _hexId);
            if (save) _storage.SaveGame(this);
        }
    }
}
