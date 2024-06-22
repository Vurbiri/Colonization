using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Players : ASingleton<Players>
{
    [Space]
    [SerializeField] private ColorsScriptable _colors;

    public Player Current => _current;
    public Player this[int index] => _players[_playerTypes[index]];
    public Player this[PlayerType index] => _players[index];

    private Player _current;
    private readonly Dictionary<PlayerType, Player> _players = new(PLAYERS_MAX);
    private readonly PlayerType[] _playerTypes = Enum<PlayerType>.GetValues();

    public const int PLAYERS_MAX = 4;
    
    protected override void Awake()
    {
        base.Awake();

        PlayerType type; int idColor;
        for (int i = 0; i < PLAYERS_MAX; i++)
        {
            type = _playerTypes[i];
            idColor = Random.Range(0, _colors.Count);
            _players[type] = new(type, i, _colors[idColor], idColor);
        }

        _current = _players[_playerTypes.Rand()];
    }

    public void SetIsland(Island island)
    {
        for (int i = 0; i < PLAYERS_MAX; i++)
            this[i].SetRoads(island.GetRoads());
    }
}
