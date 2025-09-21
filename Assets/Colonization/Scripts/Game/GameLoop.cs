using System.Collections;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    sealed public partial class GameLoop : GameEvents
    {
        private Id<GameModeId> _gameMode;
        private TurnQueue _turnQueue;
        private int _hexId;
        private GameStorage _storage;
 
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

        public IEnumerator Start_Cn()
        {
            return SetGameMode_Cn(_gameMode);
        }

        public IEnumerator Landing_Cn()
        {
            _turnQueue.Next();

            return SetGameModeNotSave_Cn(GameModeId.Landing);
        }

        public IEnumerator EndLanding_Cn()
        {
            return SetGameModeNotSave_Cn(GameModeId.EndLanding);
        }

        public IEnumerator EndTurn_Cn()
        {
            return SetGameMode_Cn(GameModeId.EndTurn);
        }

        public IEnumerator StartTurn_Cn()
        {
            _turnQueue.Next();

            return SetGameMode_Cn(GameModeId.StartTurn);
        }

        public IEnumerator WaitRoll_Cn()
        {
            return SetGameMode_Cn(GameModeId.WaitRoll);
        }

        public IEnumerator Roll_Cn(int newValue)
        {
            _hexId = newValue;
            return SetGameMode_Cn(GameModeId.Roll);
        }

        public IEnumerator Profit_Cn()
        {
            return SetGameMode_Cn(GameModeId.Profit);
        }

        public IEnumerator Play_Cn()
        {
            return SetGameMode_Cn(GameModeId.Play);
        }

        public void End__Cn(Winner winner)
        {
            _gameMode = GameModeId.End;
            _changingGameModes[GameModeId.End].Invoke(_turnQueue, _hexId);
            _storage.SaveGame(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator SetGameMode_Cn(Id<GameModeId> gameMode)
        {
            yield return null;

            _gameMode = gameMode;
            _changingGameModes[gameMode].Invoke(_turnQueue, _hexId);
            _storage.SaveGame(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator SetGameModeNotSave_Cn(Id<GameModeId> gameMode)
        {
            yield return null;

            _gameMode = gameMode;
            _changingGameModes[gameMode].Invoke(_turnQueue, _hexId);
        }
    }


}
