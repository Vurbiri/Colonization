//Assets\Colonization\Scripts\GameLoop\GameLoop.cs
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class GameLoop : MonoBehaviour
    {
        private Dices _dices;
        private TurnQueue _turnQueue;
        private Players _players;
        private InputController _inputController;
        private Hexagons _hexagons;

        public void Init(TurnQueue turnQueue, InputController inputController)
        {
            _dices = new();
            _turnQueue = turnQueue;
            _hexagons = SceneContainer.Get<Hexagons>();
            _players = SceneContainer.Get<Players>();

            _inputController = inputController;

            StartGame();
        }

        private void StartGame()
        {
            _inputController.EnableAll();
            _inputController.GameplayMap = _turnQueue.CurrentId == PlayerId.Player;
        }

        public void EndTurnPlayer()
        {
            _turnQueue.Next();
            _inputController.GameplayMap = _turnQueue.CurrentId == PlayerId.Player;

            int roll = _dices.Roll();
            UnityEngine.Debug.Log("ROLL: " + roll);
            ACurrencies free = null;
            if (roll != GATE_ID)
                free = _hexagons.GetFreeGroundResource(roll);

            _players.Profit(roll, free);
        }
    }
}
