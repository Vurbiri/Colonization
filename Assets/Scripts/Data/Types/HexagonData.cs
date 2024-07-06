using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class HexagonData
{
    [JsonProperty("k")]
    public Key Key => _key;
    [JsonProperty("i")]
    public int Id => _id;
    [JsonProperty("t")]
    public CurrencyType Type => _type;

    public SurfaceScriptable Surface { get => _surface; set => _surface = value; }
    public Vector3 Position { get => _position; set => _position = value; }

    private readonly Key _key;
    private readonly int _id;
    private SurfaceScriptable _surface;
    private Vector3 _position;
    private readonly CurrencyType _type;
    

    [JsonConstructor]
    public HexagonData(Key k, int i, CurrencyType t)
    {
        _key = k;
        _id = i;
        _type = t;
    }

    public HexagonData(Key key, int id, Vector3 position, SurfaceScriptable surface)
    {
        _key = key;
        _id = id;
        _position = position;
        _surface = surface;
        _type = surface.Type;
    }

    public void SetValues(out Key key, out int id, out CurrencyType type)
    {
        key = _key;
        id = _id;
        type = _type;
    }
}
