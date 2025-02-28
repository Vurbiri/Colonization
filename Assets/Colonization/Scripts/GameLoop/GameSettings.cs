//Assets\Colonization\Scripts\GameLoop\GameSettings.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    using static SAVE_KEYS;

    public class GameSettings
    {
        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;
        private readonly GameData _data;

        public bool IsNewGame => _data.modeStart == GameModeStart.New;
        public int MaxScore { get => _data.maxScore; }
        public IReadOnlyList<int> VisualIds => _data.visualPlayers;

        public bool IsFirstStart => _data.isFirstStart;

        public GameSettings(IReadOnlyDIContainer container)
        {
            _coroutines = container.Get<Coroutines>();
            _storage = container.Get<IStorageService>();

            if (!_storage.TryGet(SAVE_KEYS.GAME, out _data))
                _data = new();
        }

        public void Save(bool saveToFile = true, Action<bool> callback = null) 
                    => _storage.Save(SAVE_KEYS.GAME, _data, saveToFile, callback);

        public void StartGame()
        {
            _data.modeStart = GameModeStart.Continue;
        }

        public void ResetGame(bool saveToFile)
        {
            //if (IsRecord)
            //    MaxScore = Score;

            //_data.Reset();

            //_isNewRecord = false;
            Save(saveToFile);
        }

        #region Nested: GameData
        //***********************************
        [JsonObject(MemberSerialization.OptIn)]
        private class GameData
        {
            [JsonProperty(G_MODE)]
            public GameModeStart modeStart = GameModeStart.New;
            [JsonProperty(G_MAX_SCORE)]
            public int maxScore = 0;
            [JsonProperty(G_VISUAL_PLAYER_IDS)]
            public int[] visualPlayers;

            public bool isFirstStart = true;

            //[JsonConstructor]
            //public GameplayData(GameModeStart modeStart, int maxScore, int[] visualPlayers)
            //{
            //    this.modeStart = modeStart;
            //    this.maxScore = maxScore;
            //    this.visualPlayers = visualPlayers;
            //    isFirstStart = false;
            //}
            public GameData()
            {
                Reset();
                maxScore = 0;

                visualPlayers = new int[PlayerId.PlayersCount];
                for (int i = 0; i < PlayerId.PlayersCount; i++)
                    visualPlayers[i] = i;

                isFirstStart = true;
            }

            public void Reset()
            {
                modeStart = GameModeStart.New;
            }
        }
        #endregion
    }
}
