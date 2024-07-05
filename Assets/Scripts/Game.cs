using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private Island _island;
    [SerializeField] private Dices _dices;
    [Space]
    [SerializeField] private CitiesScriptable _prefabs; //test

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
        foreach (City c in _prefabs)
            c.SetCost();
    }

    private IEnumerator Start()
    {
        _island.Initialize(_gameSettings.CircleMax, _gameSettings.ChanceWater);
        _players.StartGame(_island);
        _gameSettings.StartGame();

        yield return StartCoroutine(_island.Generate_Coroutine());
        //yield return _island.Load_Wait();
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

        foreach (City c in _prefabs)
            c.Cost.Clear();
    }
}
