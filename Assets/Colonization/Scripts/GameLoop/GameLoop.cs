//Assets\Colonization\Scripts\GameLoop\GameLoop.cs
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class GameLoop : MonoBehaviour
    {
        public Listener<bool>[] AvatarSize;
        public Listener<int> Files;
        public Listener<bool> HumanAbilityId;
        public Listener Listener;

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
