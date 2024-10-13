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
        public AReadOnlyCurrenciesReactive Resources => _resources;
        public ACurrencies RoadCost => _roads.Cost;
        public IReactiveSubValues<int, CurrencyId> ExchangeRate => _exchangeRate;

        [JsonProperty(P_RESURSES)]
        private Currencies _resources;
        [JsonProperty(P_ROADS)]
        private int[][][] _roadsKey;
        [JsonProperty(P_ENDIFICES)]
        private readonly HashSet<Crossroad>[] _edifices;

        private readonly IStorageService _storage;
        private readonly Id<PlayerId> _id;
        private readonly PlayerVisual _visual;
        private readonly Roads _roads;
        private readonly StatesSet<PlayerStateId> _states;
        private readonly Currencies _exchangeRate = new();

        public Player(Id<PlayerId> playerId, PlayerVisual visual, Currencies resources, PlayerStatesScriptable states) 
                     : this(playerId, visual, states) => _resources = resources;
        public Player(Id<PlayerId> playerId, PlayerVisual visual, PlayerStatesScriptable states)
        {
            _id = playerId;
            _visual = visual;
            _roads = SceneObjects.Get<Roads>().Init(_id, _visual.color);

            int groupCount = EdificeGroupId.Count;
            _edifices = new HashSet<Crossroad>[groupCount];
            for (int i = 0; i < groupCount; i++)
                _edifices[i] = new();

            _states = states.GetAbilities();
            _storage = SceneServices.Get<IStorageService>();
        }

        public IEnumerator Save_Coroutine(bool saveToFile = true)
        {
            _roadsKey = _roads.GetCrossroadsKey();
            return _storage.Save_Coroutine(_id.ToString(), this, saveToFile, _ => _roadsKey = null);
        }

        public void Load(Crossroads crossroads)
        {
            if (_storage.TryGet(_id.ToString(), out PlayerLoadData data))
            {
                _resources = new(data.resources);
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

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            hexId = 13;


            int shrineCount = _edifices[EdificeGroupId.Shrine].Count, shrineMaxRes = _states.GetValue(PlayerStateId.ShrineMaxRes);

            _resources.AddAndClampToBlood(_states.GetValue(PlayerStateId.ShrinePassiveProfit) * shrineCount, shrineMaxRes);

            if (hexId == CONST.ID_GATE)
            {
                _resources.AddAndClampToBlood(_states.GetValue(PlayerStateId.ShrineProfit) * shrineCount, shrineMaxRes);
                _resources.ClampMain(_states.GetValue(PlayerStateId.MaxResources));
                return;
            }

            if (_states.IsMore(PlayerStateId.IsFreeGroundRes) && freeGroundRes != null)
                _resources.AddFrom(freeGroundRes);

            foreach (var port in _edifices[EdificeGroupId.Port])
                _resources.AddFrom(port.Profit(hexId, _states.GetValue(PlayerStateId.PortsRatioRes)));

            CurrenciesLite profit;
            foreach (var urban in _edifices[EdificeGroupId.Urban])
            {
                profit = urban.Profit(hexId);
                if (profit.Amount == 0 && urban.IsNotEnemy())
                    profit.RandomMainAdd(_states.GetValue(PlayerStateId.CompensationRes));
                _resources.AddFrom(profit);
            }
        }

        public void UpdateExchangeRate()
        {
            State<PlayerStateId> state = _states.GetState(PlayerStateId.ExchangeRate);

            for (int i = 0; i < CurrencyId.CountMain; i++)
                _exchangeRate.Set(i, state.NextValue);

        }

        public IReadOnlyReactiveValue<int> GetStateReactive(Id<PlayerStateId> id) => _states[id];

        public bool CanCrossroadUpgrade(Crossroad crossroad)
        {
            int upgradeGroup = crossroad.NextGroupId;
            return (crossroad.GroupId != EdificeGroupId.None || _states.IsMore(EdificeGroupId.ToIdAbility(upgradeGroup), _edifices[upgradeGroup].Count)) 
                   && crossroad.CanUpgrade(_id);
        }
        public void CrossroadUpgradeBuy(Crossroad crossroad)
        {
            if (crossroad.UpgradeBuy(_id, out ACurrencies cost))
            {
                _edifices[crossroad.GroupId].Add(crossroad);
                _resources.Pay(cost);
            }
        }

        public bool CanWallBuild(Crossroad crossroad) => _states.IsMore(PlayerStateId.IsWall) && crossroad.CanWallBuild(_id);
        public void CrossroadWallBuy(Crossroad crossroad)
        {
            if (crossroad.WallBuy(_id, out ACurrencies cost))
                _resources.Pay(cost);
        }

        public bool CanRoadBuild(Crossroad crossroad) => _states.IsMore(PlayerStateId.MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_id);
        public bool CanRoadBuy() => _resources >= _roads.Cost;
        public void RoadBuy(CrossroadLink link)
        {
            _roads.BuildAndUnion(link);
            _resources.Pay(_roads.Cost);
        }

        public bool PerkBuy(IPerk<PlayerStateId> perk)
        {
            if (perk.TargetObject == TargetOfPerkId.Player && _states.TryAddPerk(perk))
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
            [JsonProperty(P_ROADS)]
            public int[][][] roadsKey;
            [JsonProperty(P_ENDIFICES)]
            public int[][][] edifices;

            [JsonConstructor]
            public PlayerLoadData(int[] re, int[][][] ro, int[][][] en)
            {
                resources = re;
                roadsKey = ro;
                edifices = en;
            }
        }
        #endregion
    }
}
