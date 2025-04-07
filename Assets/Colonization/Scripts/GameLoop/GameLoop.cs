//Assets\Colonization\Scripts\GameLoop\GameLoop.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class GameLoop : MonoBehaviour
    {
        public UniSigner<Actor> Listener;
        public UniSigner<IList<int>> List;
        public UniSigner<bool, string, Dictionary<float, List<bool>>> Testing;
        public UniSigner Actor;

        private Dices _dices;
        private TurnQueue _turnQueue;
        private Players _players;
        private InputController _inputController;
        private Hexagons _hexagons;

        public void TestT(Actor a) => Debug.LogWarning("**********");
        public void TestT(bool b , string i , Dictionary<float, List<bool>> d) => Debug.LogWarning("gggggggg");
        public static void Test(IList<int> ints) => Debug.LogWarning("**********"+ ints);

        public void Init(TurnQueue turnQueue, InputController inputController)
        {
            _dices = new();
            _turnQueue = turnQueue;
            _hexagons = SceneContainer.Get<Hexagons>();
            _players = SceneContainer.Get<Players>();

            _inputController = inputController;
            Listener.Invoke(null);
            List.Invoke(new List<int>());
            Testing.Invoke(true, "7", null);
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
            ACurrencies free = null;
            if (roll != GATE_ID)
                free = _hexagons.GetFreeGroundResource(roll);

            _players.Profit(roll, free);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {

        }
#endif
    }


}
