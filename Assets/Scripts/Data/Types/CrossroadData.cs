using Newtonsoft.Json;

public class CrossroadData
{
    [JsonProperty("x")]
    public int X => _key.X;
    [JsonProperty("y")]
    public int Y => _key.Y;
    [JsonProperty("c")]
    public CityType Type { get; set; }
    [JsonProperty("o")]
    public PlayerType Owner { get; set; }
    [JsonIgnore]
    public Key Key => _key;

    private readonly Key _key;

    [JsonConstructor]
    public CrossroadData(int x, int y, CityType type, PlayerType owner)
    {
        _key = new(x, y);
        Type = type;
        Owner = owner;
    }

    public CrossroadData(Key key, CityType type)
    {
        _key = key;
        Type = type;
    }
}
