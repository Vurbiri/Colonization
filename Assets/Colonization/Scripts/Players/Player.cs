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
        private readonly Dictionary<EdificeGroup, HashSet<Crossroad>> _edifices = new();

        private readonly PlayerType _type;
        private readonly PlayerVisual _visual;
        private readonly Roads _roads;
        private readonly EnumHashSet<PlayerAbilityType, Ability> _abilities;
        private readonly Currencies _exchangeRate;

        public Player(PlayerType type, PlayerVisual visual, Currencies resources, Roads roads, PlayerAbilitiesScriptable abilities) : this(type, visual, roads, abilities) => _resources = resources;
        public Player(PlayerType type, PlayerVisual visual, Roads roads, PlayerAbilitiesScriptable abilities)
        {
            _type = type;
            _visual = visual;
            _blood = 0;
            _roads = roads.Initialize(_type, _visual.color);
                        
            foreach (var group in Enum<EdificeGroup>.Values)
                _edifices[group] = new();

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
            int shrineCount = _edifices[EdificeGroup.Shrine].Count, shrineMaxRes = AbilityValue(PlayerAbilityType.ShrineMaxRes);

            for (int i = 0; i < shrineCount; i++)
                _blood.Value += AbilityValue(PlayerAbilityType.ShrinePassiveProfit);
            _blood.Value = Mathf.Clamp(_blood.Value, 0, shrineMaxRes);

            if (hexId == CONST.ID_GATE)
            {
                _blood.Value = Mathf.Clamp(_blood.Value + AbilityValue(PlayerAbilityType.ShrineProfit) * shrineCount, 0, shrineMaxRes);
                return;
            }

            if (IsAbility(PlayerAbilityType.IsFreeGroundRes))
                _resources.AddFrom(freeGroundRes);

            foreach (var port in _edifices[EdificeGroup.Port])
                _resources.AddFrom(port.Profit(hexId, AbilityValue(PlayerAbilityType.PortsRatioRes)));

            Currencies profit;
            foreach (var urban in _edifices[EdificeGroup.Urban])
            {
                profit = urban.Profit(hexId);
                if (profit.Amount == 0 && urban.IsNotEnemy())
                    profit.RandomAdd(AbilityValue(PlayerAbilityType.CompensationRes));
                _resources.AddFrom(profit);
            }
        }

        public void UpdateExchangeRate()
        {
            if(!_abilities.TryGetValue((int)PlayerAbilityType.ExchangeRate, out var ability))
            {
                Debug.LogError("�� ������� ������ ExchangeRate");
                return;
            }
            
            Currencies newRate = new();
            for (int i = 0; i < newRate.Count; i++)
                newRate[i] = ability.NextValue;

            _exchangeRate.SetFrom(newRate);
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

        public bool CanWallBuild(Crossroad crossroad) => IsAbility(PlayerAbilityType.IsWall) && crossroad.CanWallBuild(_type);
        public void CrossroadWallBuy(Crossroad crossroad)
        {
            if (crossroad.WallBuy(_type, out Currencies cost))
                _resources.Pay(cost);
        }

        public bool CanRoadBuild(Crossroad crossroad) => IsAbility(PlayerAbilityType.MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_type);
        public bool CanRoadBuy() => _resources >= _roads.Cost;
        public void RoadBuy(CrossroadLink link)
        {
            _roads.BuildAndUnion(link);
            _resources.Pay(_roads.Cost);
        }

        public bool PerkBuy(IPerk perk)
        {
            if (perk.TargetObject == TargetObjectPerk.Player)
            {
                if (!_abilities.TryGetValue((int)perk.TargetAbility, out var ability))
                {
                    Debug.LogError($"�� ������� ������ {perk.TargetAbility}");
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

        private bool IsAbility(PlayerAbilityType abilityType, int value = 0) => AbilityValue(abilityType) > value;
        private int AbilityValue(PlayerAbilityType abilityType)
        {
            if (!_abilities.TryGetValue((int)abilityType, out var ability))
            {
                Debug.LogError($"�� ������� ������ {abilityType}");
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
            public Dictionary<EdificeGroup, int[][]> edifices;

            [JsonConstructor]
            public PlayerLoadData(int[] re, ReactiveValue<int> bl, int[][][] ro, Dictionary<EdificeGroup, int[][]> en)
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
