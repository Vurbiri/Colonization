using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using static JSON_KEYS;

    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IValueTypeEnum<PlayerType>
    {
        public PlayerType Type => _type;
        public Color Color => _visual.color;
        public Material MaterialLit => _visual.materialLit;
        public Material MaterialUnlit => _visual.materialUnlit;
        public Currencies Resources => _resources;

        [JsonProperty(P_RESURSES)]
        private Currencies _resources;
        [JsonProperty(P_ROADS)]
        private Key[][] _roadsKey;
        [JsonProperty(P_ENDIFICES)]
        private readonly Dictionary<EdificeGroup, HashSet<Crossroad>> _edifices = new();

        private readonly PlayerType _type;
        private readonly PlayerVisual _visual;
        private readonly Roads _roads;
        private readonly Dictionary<AbilityType, Ability> _abilities;

        public Player(PlayerType type, PlayerVisual visual, Currencies resources, Roads roads, PlayerAbilitiesScriptable abilities) : this(type, visual, roads, abilities) => _resources = resources;
        public Player(PlayerType type, PlayerVisual visual, Roads roads, PlayerAbilitiesScriptable abilities)
        {
            _type = type;
            _visual = visual;
            _roads = roads.Initialize(_type, _visual.color);
                        
            _edifices[EdificeGroup.Urban] = new();
            _edifices[EdificeGroup.Port] = new();
            _edifices[EdificeGroup.Shrine] = new();

            _abilities = new(abilities.Count);
            foreach (var ability in abilities)
                _abilities[ability.Type] = new(ability);
        }

        public IEnumerator Save_Coroutine(bool saveToFile = true)
        {
            _roadsKey = _roads.GetCrossroadsKey();
            return Storage.Save_Coroutine(_type.ToString(), this, saveToFile, _ => _roadsKey = null);
        }

        public void Load(Crossroads crossroads, Currencies resources)
        {
            if (Storage.TryLoad(_type.ToString(), out PlayerLoadData data))
            {
                _resources = new(data.resources);
                CreateRoads();
                CreateCities();
                return;
            }

            _resources = new(resources);

            #region Local: CreateRoads(), CreateCities(), CreateRoad(...)
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
                if (data.edifices == null)
                    return;

                CrossroadLoadData loadData = new();
                Crossroad crossroad;
                foreach (var dict in data.edifices)
                {
                    foreach (var arr in dict.Value)
                    {
                        loadData.SetValues(arr);
                        crossroad = crossroads.GetCrossroad(loadData.key);
                        if (crossroad.Build(_type, loadData.type, loadData.isWall))
                            _edifices[dict.Key].Add(crossroad);
                    }
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

        public void Profit(int hexId, Currencies freeGroundRes)
        {
            if (IsAbility(AbilityType.IsFreeGroundRes))
                _resources.AddFrom(freeGroundRes);

            //foreach (var city in _edifices)
            //    _resources.AddFrom(city.Profit(hexId));
        }

        public bool CanCrossroadUpgrade(Crossroad crossroad)
        {
            EdificeGroup upgradeGroup = crossroad.UpgradeGroup;
            return (crossroad.Group != EdificeGroup.None || IsAbility(upgradeGroup.ToAbilityType(), _edifices[upgradeGroup].Count)) && crossroad.CanUpgrade(_type);
        }
        public void CrossroadUpgradeBuy(Crossroad crossroad)
        {
            if (crossroad.UpgradeBuy(_type, out Currencies cost))
            {
                _edifices[crossroad.Group].Add(crossroad);
                _resources.Pay(cost);
            }
        }

        public bool CanWallBuild(Crossroad crossroad) => IsAbility(AbilityType.IsWall) && crossroad.CanWallBuild(_type);
        public void CrossroadWallBuy(Crossroad crossroad)
        {
            if (crossroad.WallBuy(_type, out Currencies cost))
                _resources.Pay(cost);
        }

        public bool CanRoadBuild(Crossroad crossroad) => IsAbility(AbilityType.MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_type);
        public bool CanRoadBuy() => _resources >= _roads.Cost;
        public void RoadBuy(CrossroadLink link)
        {
            _roads.BuildAndUnion(link);
            _resources.Pay(_roads.Cost);
        }

        public override string ToString() => $"Player: {_type}";
        
        private bool IsAbility(AbilityType abilityType, int value = 0) => _abilities.TryGetValue(abilityType, out var ability) && ability.CurrentValue > value;

        #region Nested: PlayerLoadData
        //***********************************
        [JsonObject(MemberSerialization.OptIn)]
        private class PlayerLoadData
        {
            [JsonProperty(P_RESURSES)]
            public int[] resources;
            [JsonProperty(P_ROADS)]
            public int[][][] roadsKey;
            [JsonProperty(P_ENDIFICES)]
            public Dictionary<EdificeGroup, int[][]> edifices;

            [JsonConstructor]
            public PlayerLoadData(int[] r, int[][][] k, Dictionary<EdificeGroup, int[][]> e)
            {
                resources = r;
                roadsKey = k;
                edifices = e;
            }
        }
        #endregion
    }
}
