using UnityEngine;

public class Player
{
    public PlayerType Type => _type;
    public int Id => _id;
    public int IdColor => _idColor;
    public Color Color => _color;

    private readonly PlayerType _type;
    private readonly int _id;
    private readonly int _idColor;
    private readonly Color _color;

    public Player(int id, int idColor)
    {
        _type = id.ToEnum<PlayerType>();
        _id = id;
        _idColor = idColor;
        _color = Color.white;
    }

    public Player(int id, Color color)
    {
        _type = id.ToEnum<PlayerType>();
        _id = id;
        _idColor = -1;
        _color = color;

    }

    public override string ToString() => $"Player: {_type}";
}
