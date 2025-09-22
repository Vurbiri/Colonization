using System.Collections;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    sealed public partial class GameLoop : GameEvents
    {
        private GameStorage _storage;

        private Id<GameModeId> _gameMode;
        private TurnQueue _turnQueue;
        private int _hexId;
         
        public Id<GameModeId> GameMode => _gameMode;
        public bool IsPersonTurn => _gameMode == GameModeId.Play & _turnQueue.IsPerson;

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

        public void Start() => SetGameMode(_gameMode);

        public void Landing()
        {
            _turnQueue.Next();
            SetGameModeNotSave(GameModeId.Landing);
        }
        public void EndLanding() => SetGameModeNotSave(GameModeId.EndLanding);

        public void EndTurn() => SetGameMode(GameModeId.EndTurn);
        public void StartTurn()
        {
            _turnQueue.Next();
            SetGameMode(GameModeId.StartTurn);
        }

        public void WaitRoll() => SetGameMode(GameModeId.WaitRoll);
        public void Roll(int newValue)
        {
            _hexId = newValue;
            SetGameMode(GameModeId.Roll);
        }

        public void Profit() => SetGameMode(GameModeId.Profit);

        public void Play() => SetGameMode(GameModeId.Play);

        public void End_Cn(Winner winner)
        {
            _gameMode = GameModeId.End;
            _changingGameModes[GameModeId.End].Invoke(_turnQueue, _hexId);
            _storage.SaveGame(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetGameMode(Id<GameModeId> gameMode)
        {
            SetGameMode_Cn(gameMode).Start();

            IEnumerator SetGameMode_Cn(Id<GameModeId> gameMode)
            {
                yield return null;

                _gameMode = gameMode;
                _changingGameModes[gameMode].Invoke(_turnQueue, _hexId);
                _storage.SaveGame(this);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetGameModeNotSave(Id<GameModeId> gameMode)
        {
            SetGameMode_Cn(gameMode).Start();

            IEnumerator SetGameMode_Cn(Id<GameModeId> gameMode)
            {
                yield return null;

                _gameMode = gameMode;
                _changingGameModes[gameMode].Invoke(_turnQueue, _hexId);
            }
        }
    }
}
