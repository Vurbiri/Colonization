using Newtonsoft.Json;
using static Crossroad;

public class CrossroadLoadData
{
    [JsonProperty(JP_KEY)]
    public Key key;
    [JsonProperty(JP_TYPE)]
    public CityType type;

    [JsonConstructor]
    public CrossroadLoadData(Key key, CityType type)
    {
        this.key = key;
        this.type = type;
    }
}
