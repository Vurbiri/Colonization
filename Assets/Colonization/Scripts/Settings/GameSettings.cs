//Assets\Colonization\Scripts\Settings\GameSettings.cs
using Newtonsoft.Json;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    public partial class GameSettings
    {
        private readonly GameData _data;
        private readonly Score _score;

        private PlayerVisualSetScriptable _playerVisualSet;

        public bool NewGame => _data.newGame;
        public int MaxScore => _data.maxScore;

        public bool IsFirstStart => _data.isFirstStart;


        public GameSettings(ProjectStorage storage)
        {
            _score = Score.Create(storage);
        }

        public void StartGame()
        {
            
        }

        public void ResetGame()
        {
           
        }

        #region Nested: GameData
        //***********************************
        [JsonObject(MemberSerialization.OptIn)]
        private class GameData
        {
            public bool newGame = true;
            public int maxScore = 0;

            public bool isFirstStart = true;

            public GameData(int maxScore)
            {
                newGame = false;
                this.maxScore = maxScore;
                isFirstStart = false;
            }
            public GameData()
            {
                newGame = true;
                maxScore = 0;
                isFirstStart = true;
            }

            public void Reset()
            {
                
            }
        }
        #endregion
    }
}
