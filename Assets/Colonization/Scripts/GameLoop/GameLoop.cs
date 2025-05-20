//Assets\Colonization\Scripts\GameLoop\GameLoop.cs
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    public class GameLoop : MonoBehaviour
    {
        public FileIdAndKey key;

        private Dices _dices;
        private TurnQueue _turnQueue;
        private Players _players;
        private InputController _inputController;
        private Hexagons _hexagons;

        public void Test() => Debug.Log("TEST");

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

            _dices.Roll();
            ACurrencies free = _hexagons.Profit(_dices.Preview, _dices.Current);
            _players.Profit(_dices.Current, free);

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
