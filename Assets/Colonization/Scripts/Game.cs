using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Game : MonoBehaviour
    {
        [SerializeField] private Dices _dices;

        private Players _players;

        private int _player = 0, _turn = 1;

        protected void Awake()
        {

            _players = Players.Instance;

            //Debug.Log("TEST");
            //foreach (AEdifice c in _prefabs)
            //    c.SetCost();
        }

        public void EndTurnPlayer()
        {
            StartCoroutine(_players.Current.Save_Coroutine());
            _players.Next();

            if ((_player = ++_player % MAX_PLAYERS) == 0)
                _turn++;

            int roll = _dices.Roll();
            UnityEngine.Debug.Log("ROLL: " + roll);
            ACurrencies free = null;
            //if (roll != ID_GATE)
            //    free = _island.Land.GetFreeGroundResource(roll);

            _players.Profit(roll, free);
        }

        protected void OnDestroy()
        {
            if (Players.Instance != null)
                _players.DestroyGame();
        }

        
    }
}
