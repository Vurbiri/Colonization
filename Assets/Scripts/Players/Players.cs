using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Players : ASingleton<Players>
{
    [Space]
    [SerializeField] private ColorsScriptable _colors;

    public Player Current => _current;
    public Player this[int index] => _players[(PlayerType)index];
    public Player this[PlayerType index] => _players[index];

    private Player _current;
    private readonly Dictionary<PlayerType, Player> _players = new(PLAYERS_MAX);

    public const int PLAYERS_MAX = 4;
    
    protected override void Awake()
    {
        base.Awake();

        PlayerType type; int idColor;
        for (int i = 0; i < PLAYERS_MAX; i++)
        {
            type = (PlayerType)i;
            idColor = Random.Range(0, _colors.Count);
            _players[type] = new(type, i, _colors[idColor], idColor);
        }

        _current = _players[Enum<PlayerType>.Rand(0, PLAYERS_MAX)];
    }

    public void SetIsland(Island island)
    {
        for (int i = 0; i < PLAYERS_MAX; i++)
            this[i].SetRoads(island.GetRoads());
    }
}
