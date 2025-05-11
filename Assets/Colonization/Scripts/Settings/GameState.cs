//Assets\Colonization\Scripts\Settings\GameState.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class GameState
    {
        private readonly GameData _data;
        private readonly Score _score;

        private readonly ProjectStorage _storage;

        public bool NewGame => _data.newGame;
        public Score Score => _score;

        public bool IsFirstStart => _data.isFirstStart;

        public GameState(ProjectStorage storage)
        {
            _score = Score.Create(storage);
            if (!storage.TryLoadAndBindGameData(out _data))
                storage.GameDataBind(_data = new(), true);

            _storage = storage;
        }

        public void StartGame()
        {
            _data.Start();
            _storage.Save();
        }

        public void ResetGame()
        {
            _storage.Clear();
            _score.Reset();
            _data.Reset();
            _storage.Save();
        }

        #region Nested: GameData
        //***********************************
        [JsonConverter(typeof(Converter))]
        public class GameData : IReactive<GameData>
        {
            public bool newGame = true;

            public bool isFirstStart = true;

            private readonly Signer<GameData> _eventChanged = new();

            public GameData(bool newGame)
            {
                this.newGame = newGame;
                isFirstStart = false;
            }
            public GameData()
            {
                newGame = true;
                isFirstStart = true;
            }

            public void Start()
            {
                newGame = false;

                _eventChanged.Invoke(this);
            }

            public void Reset()
            {
                newGame = true;
                isFirstStart = false;

                _eventChanged.Invoke(this);
            }

            public Unsubscriber Subscribe(Action<GameData> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);
        }
        #endregion
    }
}
