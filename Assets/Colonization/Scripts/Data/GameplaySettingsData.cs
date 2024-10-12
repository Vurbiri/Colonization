using Newtonsoft.Json;
using System;

namespace Vurbiri.Colonization
{
    using static CONST;
    using static JSON_KEYS;

    public class GameplaySettingsData
    {
        private readonly string _keySave = "gsd";
        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;
        private readonly GameplayData _data;

        public bool IsNewGame => _data.modeStart == GameModeStart.New;
        public int ChanceWater => _data.chanceWater;
        public int MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value); } }
        public int[] VisualPlayersIds => _data.visualPlayers;

        public bool IsFirstStart => _data.isFirstStart;

        public event Action<int> EventChangeMaxScore;

        public GameplaySettingsData(IReadOnlyDIContainer container, string keySave, int chanceWater)
        {
            _keySave = keySave;
            _coroutines = container.Get<Coroutines>();
            _storage = container.Get<IStorageService>();

            if (!_storage.TryGet(_keySave, out _data))
                _data = new(chanceWater);
        }

        public void Save(bool saveToFile, Action<bool> callback = null) => _coroutines.Run(_storage.Save_Coroutine(_keySave, _data, saveToFile, callback));

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

        #region Nested: GameplayData
        //***********************************
        [JsonObject(MemberSerialization.OptIn)]
        private class GameplayData
        {
            [JsonProperty(G_MODE)]
            public GameModeStart modeStart = GameModeStart.New;
            [JsonProperty(G_CHANCE_WATER)]
            public int chanceWater = 36;
            [JsonProperty(G_MAX_SCORE)]
            public int maxScore = 0;
            [JsonProperty(G_VISUAL_PLAYERS)]
            public int[] visualPlayers;

            public bool isFirstStart = true;

            [JsonConstructor]
            public GameplayData(GameModeStart modeStart, int chanceWater, int maxScore, int[] visualPlayers)
            {
                this.modeStart = modeStart;
                this.chanceWater = chanceWater;
                this.maxScore = maxScore;
                this.visualPlayers = visualPlayers;
                isFirstStart = false;
            }
            public GameplayData(int chanceWater)
            {
                Reset();
                this.chanceWater = chanceWater;
                maxScore = 0;

                visualPlayers = new int[MAX_PLAYERS];
                for (int i = 0; i < MAX_PLAYERS; i++)
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
