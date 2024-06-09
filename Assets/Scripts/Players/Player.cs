
using System;

public class Player
{
    public PlayerType Type => _type;
    public int Id => _id;
    public int IdColor => _idColor;

    private readonly PlayerType _type;
    private readonly int _id;
    private readonly int _idColor;

    public Player(PlayerType type, int idColor)
    {
        _type = type;
        _id = type.ToInt();
        _idColor = idColor;
    }

    public override string ToString() => $"Player: {_type}";
}
