//Assets\Colonization\Scripts\Settings\GameSettings.cs
using Newtonsoft.Json;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    public class GameSettings
    {
        private readonly GameData _data;
        private readonly Score _score;

        private PlayerVisualSetScriptable _playerVisualSet;

        public bool NewGame => _data.newGame;
        public int MaxScore => _data.maxScore;

        public bool IsFirstStart => _data.isFirstStart;

        public PlayersVisual PlayersVisual
        {
            get 
            {
                PlayersVisual temp = _playerVisualSet.Get(_data.colorId);
                _playerVisualSet.Dispose(); _playerVisualSet = null;
                return temp;
            }
        }

        public GameSettings(ProjectStorage storage, PlayerVisualSetScriptable playerVisualSet)
        {
            _playerVisualSet = playerVisualSet;
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
            public readonly int[] colorId;

            public bool isFirstStart = true;

            public GameData(int maxScore, int[] colorId)
            {
                newGame = false;
                this.maxScore = maxScore;
                this.colorId = colorId;
                isFirstStart = false;
            }
            public GameData()
            {
                newGame = true;
                maxScore = 0;
                colorId = new int[PlayerId.HumansCount];
                isFirstStart = true;
            }

            public void Reset()
            {
                
            }
        }
        #endregion
    }
}
