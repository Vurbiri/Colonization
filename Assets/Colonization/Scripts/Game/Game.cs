using System.Collections;
using UnityEngine;
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
        private Coroutines _coroutines;

        public Id<GameModeId> GameMode => _gameMode;

        private Game() : this(GameModeId.Init, new(PlayerId.Player), -1) { }
        private Game(Id<GameModeId> gameMode, TurnQueue turnQueue, int hexId) : base()
        {
            _gameMode = gameMode;
            _turnQueue = turnQueue;
            _hexId = hexId;
        }

        public static Game Create(GameplayStorage storage, Coroutines coroutines)
        {
            if (s_instance == null)
            {
                if (!storage.TryGetGame(out s_instance))
                    s_instance = new();

                s_instance._storage = storage;
                s_instance._coroutines = coroutines;
            }
            return s_instance;
        }

        public void Start()
        {
            _coroutines.Run(Change_Cn(_gameMode));
        }

        public void Init()
        {
            _turnQueue.Next();

            _coroutines.Run(Change_Cn(GameModeId.Init));
        }

        public void Play()
        {
            _coroutines.Run(Change_Cn(GameModeId.Play));
        }

        public void EndTurn()
        {
            _coroutines.Run(Change_Cn(GameModeId.EndTurn));

            // !!!!!!!!!!!! TEMP
            StartTurn();
        }

        public void StartTurn()
        {
            _turnQueue.Next();

            _coroutines.Run(Change_Cn(GameModeId.StartTurn));

            // !!!!!!!!!!!! TEMP
            WaitRoll();
        }

        public void WaitRoll()
        {
            _coroutines.Run(Change_Cn(GameModeId.WaitRoll));

            // !!!!!!!!!!!! TEMP
            Roll(Random.Range(3, 16));
        }

        public void Roll(int newValue)
        {
            _hexId = newValue;

            _coroutines.Run(Change_Cn(GameModeId.Roll));

            // !!!!!!!!!!!! TEMP
            Profit();
        }

        public void Profit()
        {
            _coroutines.Run(Change_Cn(GameModeId.Profit));

            // !!!!!!!!!!!! TEMP
            Play();
        }

        public void End(Winner winner)
        {
            _coroutines.Run(Change_Cn(GameModeId.End));
        }

        private IEnumerator Change_Cn(Id<GameModeId> gameMode)
        {
            yield return null;

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
