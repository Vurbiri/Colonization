//Assets\Colonization\Scripts\Settings\GameSettings.cs
using Newtonsoft.Json;
using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class GameSettings
    {
        private readonly GameData _data;
        private readonly Score _score;

        private readonly ProjectStorage _storage;

        public bool NewGame => _data.newGame;
        public int MaxScore => _data.maxScore;
        public Score Score => _score;

        public bool IsFirstStart => _data.isFirstStart;

        public GameSettings(ProjectStorage storage)
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
            _data.Reset(_score.Reset());
            _storage.Save();
        }

        #region Nested: GameData
        //***********************************
        [JsonConverter(typeof(Converter))]
        public class GameData : IReactive<GameData>
        {
            public bool newGame = true;
            public int maxScore = 0;

            public bool isFirstStart = true;

            private readonly Signer<GameData> _eventChanged = new();

            public GameData(bool newGame, int maxScore)
            {
                this.newGame = newGame;
                this.maxScore = maxScore;
                isFirstStart = false;
            }
            public GameData()
            {
                newGame = true;
                maxScore = 0;
                isFirstStart = true;
            }

            public void Start()
            {
                newGame = false;

                _eventChanged.Invoke(this);
            }

            public void Reset(int score)
            {
                newGame = true;
                maxScore = Math.Max(maxScore, score);
                isFirstStart = false;

                _eventChanged.Invoke(this);
            }

            public Unsubscriber Subscribe(Action<GameData> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);
        }
        #endregion
    }
}
