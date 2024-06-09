
public class Players : ASingleton<Players>
{
    public Player Current => _current;

    private int _currentIndex;
    private Player _current;
    private Player[] _players = new Player[PLAYERS_MAX];

    public const int PLAYERS_MAX = 4;
    
    protected override void Awake()
    {
        base.Awake();
        
        for (int i = 0; i < PLAYERS_MAX; i++)
            _players[i] = new(i.ToEnum<PlayerType>(), i);

        _currentIndex = 0;
        _current = _players[_currentIndex];
    }
}
