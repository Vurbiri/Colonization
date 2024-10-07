using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Game : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        [SerializeField] private IslandCreator _island;
        [SerializeField] private Dices _dices;
        [Space] //test
        [SerializeField] private bool _load;
        [SerializeField] private EdificesScriptable _prefabs;
        [SerializeField] private Id<PlayerId> _id;

        private GameSettingsData _gameSettings;
        private Players _players;
        private EventBus _eventBus;

        private int _player = 0, _turn = 1;

        private void Awake()
        {
            _gameSettings = GameSettingsData.Instance;
            _players = Players.Instance;
            _eventBus = EventBus.Instance;

            //Debug.Log("TEST");
            //foreach (AEdifice c in _prefabs)
            //    c.SetCost();
        }

        private IEnumerator Start()
        {
            _island.Initialize(_gameSettings.CircleMax, _gameSettings.ChanceWater);

            _gameSettings.StartGame();

            if (_load)
            {
                WaitResult<bool> waitResult = _island.Load_Wait();
                yield return waitResult;
                if (waitResult.Result)
                    _players.LoadGame(_island);
            }
            else
            {
                yield return StartCoroutine(_island.Generate_Coroutine(false));
                _players.StartGame(_island);
            }

            Destroy(_island);

            _eventBus.TriggerEndSceneCreate();

            for (int i = 0; i < 15; i ++)
                yield return null;

            GC.Collect();

            yield return null;

            UnityEngine.Debug.Log("Start");
        }

        public void EndTurnPlayer()
        {
            StartCoroutine(_players.Current.Save_Coroutine());
            _players.Next();

            if ((_player = ++_player % MAX_PLAYERS) == 0)
                _turn++;

            int roll = _dices.Roll();
            UnityEngine.Debug.Log("ROLL: " + roll);
            Currencies free = null;
            if (roll != ID_GATE)
                free = _island.Land.GetFreeGroundResource(roll);

            _players.Profit(roll, free);
        }

        private void OnDestroy()
        {
            if (Players.Instance != null)
                _players.DestroyGame();

            UnityEngine.Debug.Log("TEST (Game)");
            foreach (AEdifice c in _prefabs)
                c.Cost.Clear();
        }
    }
}
