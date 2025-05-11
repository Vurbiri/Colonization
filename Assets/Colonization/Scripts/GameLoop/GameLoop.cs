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
            _inputController.GameplayMap = _turnQueue.IsCurrentPlayer;
        }

        public void EndTurnPlayer()
        {
            _players.EndTurn(_turnQueue.CurrentId.Value);

            int roll = _dices.Roll();
            ACurrencies free = null;
            if (roll != GATE_ID)
                free = _hexagons.GetFreeGroundResource(roll);

            _players.Profit(roll, free);

            _turnQueue.Next();

            _players.StartTurn(_turnQueue.CurrentId.Value);
            _inputController.GameplayMap = _turnQueue.IsCurrentPlayer;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            
        }
#endif
    }


}
