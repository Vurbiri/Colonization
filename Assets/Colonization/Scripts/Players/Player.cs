using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static JSON_KEYS;

    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IValueId<PlayerId>
    {
        public Id<PlayerId> Id => _id;
        public Color Color => _visual.color;
        public Material MaterialLit => _visual.materialLit;
        public Material MaterialUnlit => _visual.materialUnlit;
        public Currencies Resources => _resources;
        public Currencies ExchangeRate => _exchangeRate;

        [JsonProperty(P_RESURSES)]
        private Currencies _resources;
        [JsonProperty(P_BLOOD)]
        private ReactiveValue<int> _blood;
        [JsonProperty(P_ROADS)]
        private int[][][] _roadsKey;
        [JsonProperty(P_ENDIFICES)]
        private readonly HashSet<Crossroad>[] _edifices;

        private readonly Id<PlayerId> _id;
        private readonly PlayerVisual _visual;
        private readonly Roads _roads;
        private readonly AbilitySet<PlayerAbilityId> _abilities;
        private readonly Currencies _exchangeRate;

        public Player(Id<PlayerId> playerId, PlayerVisual visual, Currencies resources, Roads roads, PlayerAbilitiesScriptable abilities) 
                     : this(playerId, visual, roads, abilities) => _resources = resources;
        public Player(Id<PlayerId> playerId, PlayerVisual visual, Roads roads, PlayerAbilitiesScriptable abilities)
        {
            _id = playerId;
            _visual = visual;
            _blood = 0;
            _roads = roads.Initialize(_id, _visual.color);

            int groupCount = EdificeGroupId.Count;
            _edifices = new HashSet<Crossroad>[groupCount];
            for (int i = 0; i < groupCount; i++)
                _edifices[i] = new();

            _abilities = abilities.GetAbilities();
        }

        public IEnumerator Save_Coroutine(bool saveToFile = true)
        {
            _roadsKey = _roads.GetCrossroadsKey();
            return Storage.Save_Coroutine(_id.ToString(), this, saveToFile, _ => _roadsKey = null);
        }

        public void Load(Crossroads crossroads)
        {
            if (Storage.TryLoad(_id.ToString(), out PlayerLoadData data))
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
                for(int i = 0; i < EdificeGroupId.Count; i++)
                {
                    foreach (var arr in data.edifices[i])
                    {
                        loadData.SetValues(arr);
                        crossroad = crossroads.GetCrossroad(loadData.key);
                        if (crossroad.Build(_id, loadData.id, loadData.isWall))
                            _edifices[i].Add(crossroad);
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
            int shrineCount = _edifices[EdificeGroupId.Shrine].Count, shrineMaxRes = _abilities.GetValue(PlayerAbilityId.ShrineMaxRes);

            for (int i = 0; i < shrineCount; i++)
                _blood.Value += _abilities.GetValue(PlayerAbilityId.ShrinePassiveProfit);
            _blood.Value = Mathf.Clamp(_blood.Value, 0, shrineMaxRes);

            if (hexId == CONST.ID_GATE)
            {
                _blood.Value = Mathf.Clamp(_blood.Value + _abilities.GetValue(PlayerAbilityId.ShrineProfit) * shrineCount, 0, shrineMaxRes);
                _resources.ClampMain(_abilities.GetValue(PlayerAbilityId.MaxResources));
                return;
            }

            if (_abilities.IsMore(PlayerAbilityId.IsFreeGroundRes))
                _resources.AddFrom(freeGroundRes);

            foreach (var port in _edifices[EdificeGroupId.Port])
                _resources.AddFrom(port.Profit(hexId, _abilities.GetValue(PlayerAbilityId.PortsRatioRes)));

            Currencies profit;
            foreach (var urban in _edifices[EdificeGroupId.Urban])
            {
                profit = urban.Profit(hexId);
                if (profit.Amount == 0 && urban.IsNotEnemy())
                    profit.RandomMainAdd(_abilities.GetValue(PlayerAbilityId.CompensationRes));
                _resources.AddFrom(profit);
            }
        }

        public void UpdateExchangeRate()
        {
            Ability<PlayerAbilityId> ability = _abilities[PlayerAbilityId.ExchangeRate];

            Currencies newRate = new();
            for (int i = 0; i < newRate.CountMain; i++)
                newRate[i] = ability.NextValue;

            _exchangeRate.SetFrom(newRate);
        }

        public bool CanCrossroadUpgrade(Crossroad crossroad)
        {
            int upgradeGroup = crossroad.NextGroupId;
            return (crossroad.GroupId != EdificeGroupId.None || _abilities.IsMore(EdificeGroupId.ToIdAbility(upgradeGroup), _edifices[upgradeGroup].Count)) 
                   && crossroad.CanUpgrade(_id);
        }
        public void CrossroadUpgradeBuy(Crossroad crossroad)
        {
            if (crossroad.UpgradeBuy(_id, out Currencies cost))
            {
                _edifices[crossroad.GroupId].Add(crossroad);
                _resources.Pay(cost);
            }
        }

        public bool CanWallBuild(Crossroad crossroad) => _abilities.IsMore(PlayerAbilityId.IsWall) && crossroad.CanWallBuild(_id);
        public void CrossroadWallBuy(Crossroad crossroad)
        {
            if (crossroad.WallBuy(_id, out Currencies cost))
                _resources.Pay(cost);
        }

        public bool CanRoadBuild(Crossroad crossroad) => _abilities.IsMore(PlayerAbilityId.MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_id);
        public bool CanRoadBuy() => _resources >= _roads.Cost;
        public void RoadBuy(CrossroadLink link)
        {
            _roads.BuildAndUnion(link);
            _resources.Pay(_roads.Cost);
        }

        public bool PerkBuy(IPerk<PlayerAbilityId> perk)
        {
            if (perk.TargetObject == TargetOfPerkId.Player && _abilities.TryAddPerk(perk))
            {
                _resources.Pay(perk.Cost);
                return true;
            }

            return false;
        }

        public override string ToString() => $"Player: {_id}";


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
            public int[][][] edifices;

            [JsonConstructor]
            public PlayerLoadData(int[] re, ReactiveValue<int> bl, int[][][] ro, int[][][] en)
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
