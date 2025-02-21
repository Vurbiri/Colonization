//Assets\Colonization\Scripts\GameLoop\GameLoop.cs
using UnityEngine;
using Vurbiri.Colonization.Data;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class GameLoop : MonoBehaviour
    {
        private Dices _dices;
        private Players _players;
        private PlayersData _playersData;
        private Land _land;

        public void Init()
        {
            _dices = new();

            _land = SceneObjects.Get<Land>();
            _players = SceneObjects.Get<Players>();
            _playersData = SceneData.Get<PlayersData>();
        }

        public void EndTurnPlayer()
        {
            _players.Next();

            int roll = _dices.Roll();
            UnityEngine.Debug.Log("ROLL: " + roll);
            ACurrencies free = null;
            if (roll != ID_GATE)
                free = _land.GetFreeGroundResource(roll);

            _players.Profit(roll, free);

            _playersData.Save();
        }
    }
}
