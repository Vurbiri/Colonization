using UnityEngine;

public class Player
{
    public PlayerType Type => _type;
    public int Id => _id;
    public int IdColor => _idColor;
    public Color Color => _color;

    public bool IsBuildRoad => _roads.IsEmpty;

    private readonly PlayerType _type;
    private readonly int _id;
    private readonly int _idColor;
    private readonly Color _color;

    private Roads _roads;

    public Player(PlayerType type, int id, Color color, int idColor)
    {
        _type = type;
        _id = id;
        _idColor = idColor;
        _color = color;
    }

    public void SetRoads(Roads roads) => _roads = roads.Initialize(_type, _color);
    public void BuildRoad(CrossroadLink link) => _roads.BuildRoad(link);

    public override string ToString() => $"Player: {_type}";
}
