using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DefaultExecutionOrder(-1)]
public class Players : ASingleton<Players>, IEnumerable<Player>
{
    [Space]
    [SerializeField] private Currencies _startResources;
    [Space]
    [SerializeField] private PlayerVisualSetScriptable _visualSet;

    public Player Current => _current;
    public Player this[PlayerType type] => _players[type];
    public PlayerVisualSetScriptable VisualSet => _visualSet;

    private Player _current;
    private readonly EnumHashSet<PlayerType, Player> _players = new();

    public const int MAX = 4;

    public void StartGame(Island island)
    {
        int[] idVisuals = _visualSet.RandIds(MAX);
        PlayerType type; int idVisual;
        for (int i = 0; i < MAX; i++)
        {
            type = (PlayerType)i;
            idVisual = idVisuals[i];
            _players.Add(new(type, _visualSet.Get(idVisual), new(_startResources), island.GetRoads()));
        }

        _current = _players[Enum<PlayerType>.Rand(0, MAX)];
    }

    public void Next() => _current = _players.Next(_current.Type);

    public void Receipt(int hexId)
    {
        foreach(Player player in _players)
            player.Receipt(hexId);
    }

    public void DestroyGame()
    {
        
    }

    public IEnumerator<Player> GetEnumerator() => _players.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _players.GetEnumerator();
}
