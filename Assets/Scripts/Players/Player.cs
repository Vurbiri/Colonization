using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IValueTypeEnum<PlayerType>
{
    public PlayerType Type => _type;
    public Color Color => _visual.color;
    public Material Material => _visual.material;
    public Currencies Resources => _data.resources;

    private readonly PlayerType _type;
    private readonly PlayerVisual _visual;
    private readonly PlayerData _data;
    private readonly Roads _roads;

    public Player(PlayerType type, PlayerVisual visual, Currencies resources, Roads roads)
    {
        _type = type;
        _visual = visual;
        _roads = roads.Initialize(_type, _visual.color);
        _data = new(resources);
    }

    private Player(PlayerType type, PlayerVisual visual, Roads roads, Crossroads crossroad, PlayerData data)
    {
        _type = type;
        _visual = visual;
        _roads = roads.Initialize(_type, _visual.color);

        _data = data;
    }

    public void Receipt(int hexId)
    {
        foreach (var city in _data.cities)
            _data.resources.AddFrom(city.Profit(hexId));
    }

    public void BuildRoad(CrossroadLink link)
    {
        _roads.BuildAndUnion(link);
        _data.resources.Pay(_roads.Cost);

    }
    public bool CanRoadBuilt(Crossroad crossroad) => _data.resources >= _roads.Cost && crossroad.CanRoadBuilt(_type);

    public void CityBuild(Crossroad crossroad, CityType type)
    {
        if(!_data.cities.Contains(crossroad) && crossroad.Build(_type, type, out Currencies cost))
        {
            _data.Add(crossroad);
            _data.resources.Pay(cost);
        }
    }
    public void CityUpgrade(Crossroad crossroad)
    {
        if (_data.cities.Contains(crossroad) && crossroad.Upgrade(out Currencies cost))
            _data.resources.Pay(cost);
    }

    public IEnumerator Save_Coroutine()
    {
        _data.roads = _roads.GetCrossroadsKey();
        return Storage.Save_Coroutine(_type.ToString(), _data);
    }

    public static bool Load(PlayerType type, PlayerVisual visual, Roads roads, Crossroads crossroad, out Player player)
    {
        Return<PlayerData> loading = Storage.Load<PlayerData>(type.ToString());
        if (loading.Result)
        {
            player = new(type, visual, roads, crossroad, loading.Value);
            return true;
        }
        player = null;
        return false;
    }


    public override string ToString() => $"Player: {_type}";


    #region Nested: GameSave
    //***********************************
    private class PlayerData
    {
        [JsonProperty("r")]
        public Currencies resources;
        [JsonProperty("c")]
        public Key[][] roads;
        [JsonProperty("d")]
        public List<CrossroadData> crossData;

        [JsonIgnore]
        public readonly HashSet<Crossroad> cities = new();

        public PlayerData(Currencies resources)
        {
            this.resources = resources;
            crossData = new();
        }

        public void Add(Crossroad crossroad)
        {
            cities.Add(crossroad);
            crossData.Add(crossroad.GetData());

        }
    }
    #endregion
}
