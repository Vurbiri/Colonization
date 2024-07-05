using Newtonsoft.Json;
using UnityEngine;

public class HexagonData
{
    [JsonProperty("x")]
    public int X => _key.X;
    [JsonProperty("y")]
    public int Y => _key.Y;
    [JsonProperty("i")]
    public int Id { get; }
    [JsonProperty("t")]
    public CurrencyType Type => _type;
    [JsonIgnore]
    public Key key => _key;
    [JsonIgnore]
    public SurfaceScriptable Surface { get => _surface; set => _surface = value; }
    [JsonIgnore]
    public Vector3 Position { get => _position; set => _position = value; }
    

    private SurfaceScriptable _surface;
    private Vector3 _position;
    private readonly CurrencyType _type;
    private readonly Key _key;

    [JsonConstructor]
    public HexagonData(int x, int y, int id, CurrencyType type)
    {
        _key = new(x, y);
        Id = id;
        _type = type;
    }

    public HexagonData(Key key, int id, Vector3 position, SurfaceScriptable surface)
    {
        _key = key;
        Id = id;
        _position = position;
        _surface = surface;
        _type = surface.Type;
    }
}
