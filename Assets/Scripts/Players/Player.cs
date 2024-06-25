using System.Collections.Generic;
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

    private Roads _roads;
    private List<Crossroad> _cities = new();

    public Player(PlayerType type, int id, Color color, int idColor)
    {
        _type = type;
        _id = id;
        _idColor = idColor;
        _color = color;
    }

    public void SetRoads(Roads roads) => _roads = roads.Initialize(_type, _color);
    public void BuildRoad(CrossroadLink link) => _roads.BuildRoad(link);
    public bool CanRoadBuilt(Crossroad crossroad)
    {
        return crossroad.CanRoadBuilt(_type);
    }

    public bool CanCityBuilt(Crossroad crossroad)
    {
        if (!crossroad.IsNotCitiesNearby())
            return false;
        
        return _cities.Count == 0 || crossroad.IsRoadConnect(_type);
    }
    public void BuildCity(Crossroad crossroad)
    {
        if(crossroad.Upgrade(_type))
            _cities.Add(crossroad);
    }

    public override string ToString() => $"Player: {_type}";
}
