using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vurbiri.Colonization.Crossroad;

namespace Vurbiri.Colonization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IValueTypeEnum<PlayerType>
    {
        public PlayerType Type => _type;
        public Color Color => _visual.color;
        public Material Material => _visual.material;
        public Currencies Resources => _resources;

        [JsonProperty(JP_RES)]
        private Currencies _resources;
        [JsonProperty(JP_ROAD)]
        private Key[][] _roadsKey;
        [JsonProperty(JP_CITY)]
        private readonly HashSet<Crossroad> _cities = new();

        private readonly PlayerType _type;
        private readonly PlayerVisual _visual;
        private readonly Roads _roads;

        private const string JP_RES = "r", JP_ROAD = "k", JP_CITY = "c";

        public Player(PlayerType type, PlayerVisual visual, Currencies resources, Roads roads) : this(type, visual, roads) => _resources = resources;
        public Player(PlayerType type, PlayerVisual visual, Roads roads)
        {
            _type = type;
            _visual = visual;
            _roads = roads.Initialize(_type, _visual.color);

        }
        public IEnumerator Save_Coroutine(bool saveToFile = true)
        {
            _roadsKey = _roads.GetCrossroadsKey();
            return Storage.Save_Coroutine(_type.ToString(), this, saveToFile, _ => _roadsKey = null);
        }

        public void Load(Crossroads crossroads, Currencies resources)
        {
            PlayerLoadData data;
            Return<PlayerLoadData> loading = Storage.Load<PlayerLoadData>(_type.ToString());
            if (loading.Result)
            {
                data = loading.Value;
                _resources = new(data.resources);
                CreateRoads();
                CreateCities();
                return;
            }

            _resources = new(resources);

            #region Local: CreateRoad(...)
            //=================================
            void CreateRoads()
            {
                if (data.roadsKey == null)
                    return;

                foreach (var k in data.roadsKey)
                    CreateRoad(k);
            }
            //=================================
            void CreateCities()
            {
                if (data.cities == null)
                    return;

                CrossroadLoadData loadData = new();
                Crossroad crossroad;
                foreach (var arr in data.cities)
                {
                    loadData.SetValues(arr);
                    crossroad = crossroads.GetCrossroad(loadData.key);
                    if (crossroad.Build(_type, loadData.type))
                        _cities.Add(crossroad);
                }

                _roads.SetRoadsEndings();
            }
            //=================================
            void CreateRoad(int[][] keys)
            {
                int count = keys.Length;
                if (count < 2) return;

                Key key = new();
                Crossroad start = crossroads.GetCrossroad(key.SetValues(keys[0]));
                for (int i = 1; i < count; i++)
                {
                    foreach (var link in start.Links)
                    {
                        if (link.Contains(key.SetValues(keys[i])))
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

        public void CityUpgrade(Crossroad crossroad)
        {
            if (crossroad.Upgrade(_type, out Currencies cost))
            {
                _cities.Add(crossroad);
                _resources.Pay(cost);
            }
        }

        public override string ToString() => $"Player: {_type}";


        #region Nested: PlayerLoadData
        //***********************************
        private class PlayerLoadData
        {
            [JsonProperty(JP_RES)]
            public int[] resources;
            [JsonProperty(JP_ROAD)]
            public int[][][] roadsKey;
            [JsonProperty(JP_CITY)]
            public int[][] cities;

            [JsonConstructor]
            public PlayerLoadData(int[] r, int[][][] k, int[][] c)
            {
                resources = r;
                roadsKey = k;
                cities = c;
            }
        }
        #endregion
    }
}
