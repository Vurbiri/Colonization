using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Game : MonoBehaviour
    {
        [SerializeField] private Dices _dices;

        private Players _players;
        private Land _land;

        private int _player = 0, _turn = 1;

        public void Init()
        {
            _land = SceneObjects.Get<Land>();
            _players = SceneObjects.Get<Players>();
        }

        public void EndTurnPlayer()
        {
            _players.Save();
            _players.Next();

            if ((_player = ++_player % PlayerId.CountPlayers) == 0)
                _turn++;

            int roll = _dices.Roll();
            UnityEngine.Debug.Log("ROLL: " + roll);
            ACurrencies free = null;
            if (roll != ID_GATE)
                free = _land.GetFreeGroundResource(roll);

            _players.Profit(roll, free);
        }
    }
}
