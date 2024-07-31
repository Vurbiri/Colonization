using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        [SerializeField] private Island _island;
        [SerializeField] private Dices _dices;
        [Space] //test
        [SerializeField] private bool _load;
        [SerializeField] private EdificesScriptable _prefabs;

        private GameSettingsData _gameSettings;
        private Players _players;
        private EventBus _eventBus;

        private int _player = 0, _turn = 1;

        private void Awake()
        {
            _gameSettings = GameSettingsData.Instance;
            _players = Players.Instance;
            _eventBus = EventBus.Instance;

            Debug.Log("TEST");
            foreach (AEdifice c in _prefabs)
                c.SetCost();
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
                {
                    _players.LoadGame(_island);
                    yield break;
                }
            }

            yield return StartCoroutine(_island.Generate_Coroutine(false));
            _players.StartGame(_island);
        }

        public void EndTurnPlayer()
        {
            StartCoroutine(_players.Current.Save_Coroutine());
            _players.Next();

            if ((_player = ++_player % Players.MAX) == 0)
                _turn++;

            int roll = _dices.Roll();
            Debug.Log("ROLL: " + roll);
            _players.Receipt(roll);
        }

        private void OnDestroy()
        {
            if (Players.Instance != null)
                _players.DestroyGame();
        }
    }
}