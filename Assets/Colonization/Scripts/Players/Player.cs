using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

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
        public AReactive<Currencies> ExchangeRate => _exchangeRate;

        [JsonProperty(P_RESURSES)]
        private Currencies _resources;
        [JsonProperty(P_BLOOD)]
        private ReactiveValue<int> _blood;
        [JsonProperty(P_ROADS)]
        private int[][][] _roadsKey;
        [JsonProperty(P_ENDIFICES)]
        private readonly Dictionary<int, HashSet<Crossroad>> _edifices = new();

        private readonly PlayerType _type;
        private readonly PlayerVisual _visual;
        private readonly Roads _roads;
        private readonly IdHashSet<IdPlayerAbility, Ability> _abilities;
        private readonly Currencies _exchangeRate;

        public Player(PlayerType type, PlayerVisual visual, Currencies resources, Roads roads, PlayerAbilitiesScriptable abilities) : this(type, visual, roads, abilities) => _resources = resources;
        public Player(PlayerType type, PlayerVisual visual, Roads roads, PlayerAbilitiesScriptable abilities)
        {
            _type = type;
            _visual = visual;
            _blood = 0;
            _roads = roads.Initialize(_type, _visual.color);
                        
            //for(int i = 0; i < EdificeGroup.Count; i++)
            //    _edifices[i] = new();

            _abilities = abilities.GetAbilities();
        }

        public IEnumerator Save_Coroutine(bool saveToFile = true)
        {
            _roadsKey = _roads.GetCrossroadsKey();
            return Storage.Save_Coroutine(_type.ToString(), this, saveToFile, _ => _roadsKey = null);
        }

        public void Load(Crossroads crossroads)
        {
            if (Storage.TryLoad(_type.ToString(), out PlayerLoadData data))
            {
                _resources = new(data.resources);
                _blood = data.blood;
                CreateRoads(data);
                CreateCities(data);
                return;
            }

            #region Local: CreateRoads(), CreateCities(), CreateRoad(...)
            //=================================
            void CreateRoads(PlayerLoadData data)
            {
                if (data.roadsKey == null)
                    return;

                foreach (var k in data.roadsKey)
                    CreateRoad(k);
            }
            //=================================
            void CreateCities(PlayerLoadData data)
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
                        if (crossroad.Build(_type, loadData.id, loadData.isWall))
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

                Key key = new(keys[0]);
                Crossroad start = crossroads.GetCrossroad(key);
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
            int shrineCount = _edifices[IdEdificeGroup.Shrine].Count, shrineMaxRes = AbilityValue(IdPlayerAbility.ShrineMaxRes);

            for (int i = 0; i < shrineCount; i++)
                _blood.Value += AbilityValue(IdPlayerAbility.ShrinePassiveProfit);
            _blood.Value = Mathf.Clamp(_blood.Value, 0, shrineMaxRes);

            if (hexId == CONST.ID_GATE)
            {
                _blood.Value = Mathf.Clamp(_blood.Value + AbilityValue(IdPlayerAbility.ShrineProfit) * shrineCount, 0, shrineMaxRes);
                return;
            }

            if (IsAbility(IdPlayerAbility.IsFreeGroundRes))
                _resources.AddFrom(freeGroundRes);

            foreach (var port in _edifices[IdEdificeGroup.Port])
                _resources.AddFrom(port.Profit(hexId, AbilityValue(IdPlayerAbility.PortsRatioRes)));

            Currencies profit;
            foreach (var urban in _edifices[IdEdificeGroup.Urban])
            {
                profit = urban.Profit(hexId);
                if (profit.Amount == 0 && urban.IsNotEnemy())
                    profit.RandomAdd(AbilityValue(IdPlayerAbility.CompensationRes));
                _resources.AddFrom(profit);
            }
        }

        public void UpdateExchangeRate()
        {
            if(!_abilities.TryGetValue(IdPlayerAbility.ExchangeRate, out var ability))
            {
                Debug.LogError("Не найдена абилка ExchangeRate");
                return;
            }
            
            Currencies newRate = new();
            for (int i = 0; i < newRate.Count; i++)
                newRate[i] = ability.NextValue;

            _exchangeRate.SetFrom(newRate);
        }

        public bool CanCrossroadUpgrade(Crossroad crossroad)
        {
            int upgradeGroup = crossroad.IdUpgradeGroup;
            return (crossroad.IdGroup != IdEdificeGroup.None || IsAbility(IdEdificeGroup.ToIdAbility(upgradeGroup), _edifices[upgradeGroup].Count)) && crossroad.CanUpgrade(_type);
        }
        public void CrossroadUpgradeBuy(Crossroad crossroad)
        {
            if (crossroad.UpgradeBuy(_type, out Currencies cost))
            {
                _edifices[crossroad.IdGroup].Add(crossroad);
                _resources.Pay(cost);
            }
        }

        public bool CanWallBuild(Crossroad crossroad) => IsAbility(IdPlayerAbility.IsWall) && crossroad.CanWallBuild(_type);
        public void CrossroadWallBuy(Crossroad crossroad)
        {
            if (crossroad.WallBuy(_type, out Currencies cost))
                _resources.Pay(cost);
        }

        public bool CanRoadBuild(Crossroad crossroad) => IsAbility(IdPlayerAbility.MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_type);
        public bool CanRoadBuy() => _resources >= _roads.Cost;
        public void RoadBuy(CrossroadLink link)
        {
            _roads.BuildAndUnion(link);
            _resources.Pay(_roads.Cost);
        }

        public bool PerkBuy(IPerk perk)
        {
            if (perk.TargetObject == IdTargetObjectPerk.Player)
            {
                if (!_abilities.TryGetValue((int)perk.TargetAbility, out var ability))
                {
                    Debug.LogError($"Не найдена абилка {perk.TargetAbility}");
                    return false;
                }

                if (ability.TryAddPerk(perk))
                {
                    _resources.Pay(perk.Cost);
                    return true;
                }
            }

            return false;
        }

        public override string ToString() => $"Player: {_type}";

        private bool IsAbility(int idAbility, int value = 0) => AbilityValue(idAbility) > value;
        private int AbilityValue(int idAbility)
        {
            if (!_abilities.TryGetValue(idAbility, out var ability))
            {
                Debug.LogError($"Не найдена абилка {idAbility}");
                return 0;
            }

            return ability.NextValue;
        }

        #region Nested: PlayerLoadData
        //***********************************
        [JsonObject(MemberSerialization.OptIn)]
        private class PlayerLoadData
        {
            [JsonProperty(P_RESURSES)]
            public int[] resources;
            [JsonProperty(P_BLOOD)]
            public ReactiveValue<int> blood;
            [JsonProperty(P_ROADS)]
            public int[][][] roadsKey;
            [JsonProperty(P_ENDIFICES)]
            public Dictionary<int, int[][]> edifices;

            [JsonConstructor]
            public PlayerLoadData(int[] re, ReactiveValue<int> bl, int[][][] ro, Dictionary<int, int[][]> en)
            {
                resources = re;
                blood = bl;
                roadsKey = ro;
                edifices = en;
            }
        }
        #endregion
    }
}
