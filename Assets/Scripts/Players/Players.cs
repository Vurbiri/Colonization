using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Players : ASingleton<Players>
{
    [Space]
    [SerializeField] private ColorsScriptable _colors;

    public Player Current => _current;

    private int _currentIndex;
    private Player _current;
    private readonly Player[] _players = new Player[PLAYERS_MAX];

    public const int PLAYERS_MAX = 4;
    
    protected override void Awake()
    {
        base.Awake();
        
        for (int i = 0; i < PLAYERS_MAX; i++)
            _players[i] = new(i, _colors.Rand);

        _currentIndex = Random.Range(0, PLAYERS_MAX);
        _current = _players[_currentIndex];
    }

    public void SetIsland(Island island)
    {
        for (int i = 0; i < PLAYERS_MAX; i++)
            _players[i].SetRoads(island.GetRoads());
    }
}
