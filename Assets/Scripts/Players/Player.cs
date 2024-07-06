using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class Player : IValueTypeEnum<PlayerType>
{
    public PlayerType Type => _type;
    public Color Color => _visual.color;
    public Material Material => _visual.material;
    public Currencies Resources => _resources;

    [JsonProperty(JP_RES)]
    private readonly Currencies _resources;
    [JsonProperty(JP_ROAD)]
    private Key[][] _roadsKey;
    [JsonProperty(JP_CITY)]
    private readonly HashSet<Crossroad> _cities = new();
    
    private readonly PlayerType _type;
    private readonly PlayerVisual _visual;
    private readonly Roads _roads;

    private const string JP_RES = "r", JP_ROAD = "k", JP_CITY = "c";

    public Player(PlayerType type, PlayerVisual visual, Currencies resources, Roads roads)
    {
        _type = type;
        _visual = visual;
        _resources = resources;
        _roads = roads.Initialize(_type, _visual.color);

    }

    private Player(PlayerType type, PlayerVisual visual, Roads roads, Crossroads crossroads, PlayerLoadData data)
    {
        _type = type;
        _visual = visual;
        _resources = new(data.resources);
        _roads = roads.Initialize(_type, _visual.color);

        CreateRoads();
        CreateCities();


        #region Local: CreateRoad(...)
        //=================================
        void CreateRoads()
        {
            if (data.roadsKey == null)
                return;
            
            foreach (var k in data.roadsKey)
                CreateRoad(k);
            _roads.TryUnion();
        }
        //=================================
        void CreateCities()
        {
            if (data.cities == null)
                return;

            Debug.Log(data.cities.Length);
            Crossroad crossroad;
            foreach (var city in data.cities)
            {
                crossroad = crossroads.GetCrossroad(city.key);
                Debug.Log(crossroad);
                if (crossroad.Build(type, city.type))
                    _cities.Add(crossroad);
            }
        }
        //=================================
        void CreateRoad(Key[] keys)
        {
            int count = keys.Length;
            if (count < 2) return;

            Crossroad start = crossroads.GetCrossroad(keys[0]);
            for (int i = 1; i < count; i++)
            {
                foreach (var link in start.Links)
                {
                    if(link.Contains(keys[i]))
                    {
                        link.SetStart(start);
                        start = link.End;
                        _roads.Build(link);
                        break;
                    }
                }
            }
        }
        #endregion
    }

    public void Receipt(int hexId)
    {
        foreach (var city in _cities)
            _resources.AddFrom(city.Profit(hexId));
    }

    public void BuildRoad(CrossroadLink link)
    {
        _roads.BuildAndUnion(link);
        _resources.Pay(_roads.Cost);

    }
    public bool CanRoadBuilt(Crossroad crossroad) => _resources >= _roads.Cost && crossroad.CanRoadBuilt(_type);

    public void CityBuild(Crossroad crossroad, CityType type)
    {
        if(!_cities.Contains(crossroad) && crossroad.Build(_type, type, out Currencies cost))
        {
            _cities.Add(crossroad);
            _resources.Pay(cost);
        }
    }
    public void CityUpgrade(Crossroad crossroad)
    {
        if (_cities.Contains(crossroad) && crossroad.Upgrade(out Currencies cost))
            _resources.Pay(cost);
    }

    public IEnumerator Save_Coroutine()
    {
        _roadsKey = _roads.GetCrossroadsKey();
        return Storage.Save_Coroutine(_type.ToString(), this, _ => _roadsKey = null);
    }

    public static Player Load(PlayerType type, PlayerVisual visual, Roads roads, Crossroads crossroad, Currencies resources)
    {
        if (Storage.ContainsKey(type.ToString()))
        {
            Return<PlayerLoadData> loading = Storage.Load<PlayerLoadData>(type.ToString());
            if (loading.Result)
                return new(type, visual, roads, crossroad, loading.Value);
        }

        return new(type, visual, new(resources), roads);
    }


    public override string ToString() => $"Player: {_type}";


    #region Nested: PlayerLoadData
    //***********************************
    private class PlayerLoadData
    {
        [JsonProperty(JP_RES)]
        public int[] resources;
        [JsonProperty(JP_ROAD)]
        public Key[][] roadsKey;
        [JsonProperty(JP_CITY)]
        public CrossroadLoadData[] cities;

        [JsonConstructor]
        public PlayerLoadData(int[] r, Key[][] k, CrossroadLoadData[] c)
        {
            resources = r;
            roadsKey = k;
            cities = c;
        }
    }
    #endregion
}
