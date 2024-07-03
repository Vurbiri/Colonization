using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private Island _island;
    [Space]
    [SerializeField] private CitiesScriptable _prefabs; //test

    private GameSettingsData _gameSettings;
    private Players _players;
    private EventBus _eventBus;

    private void Awake()
    {
        _gameSettings = GameSettingsData.Instance;
        _players = Players.Instance;
        _eventBus = EventBus.Instance;

        _players.Create();

        Debug.Log("TEST");
        foreach (City c in _prefabs)
            c.SetCost();
    }

    private IEnumerator Start()
    {
        _island.Initialize(_gameSettings.CircleMax, _gameSettings.ChanceWater);
        _players.StartGame(_island);
        _gameSettings.StartGame();

        yield return StartCoroutine(_island.Generate_Coroutine());

    }

    private void OnDestroy()
    {
        if (Players.Instance != null)
            _players.DestroyGame();

        foreach (City c in _prefabs)
            c.Cost.Clear();
    }
}
