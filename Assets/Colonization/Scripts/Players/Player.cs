using Newtonsoft.Json;
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IValueId<PlayerId>
    {
        public Id<PlayerId> Id => _id;
        public Color Color => _visual.color;
        public Material MaterialLit => _visual.materialLit;
        public Material MaterialUnlit => _visual.materialUnlit;
        public AReadOnlyCurrenciesReactive Resources => _data.resources;
        public ACurrencies RoadCost => _roads.Cost;
        public IReactiveSubValues<int, CurrencyId> ExchangeRate => _exchangeRate;

        private PlayerData _data;
        private readonly Id<PlayerId> _id;
        private readonly PlayerVisual _visual;
        private readonly Roads _roads;
        private readonly StatesSet<PlayerStateId> _states;
        private readonly Currencies _exchangeRate = new();

        public Player(Id<PlayerId> playerId, PlayerVisual visual, StatesSet<PlayerStateId> states, Roads roads)
        {
            _id = playerId;
            _visual = visual;
            _roads = roads.Init(_id, _visual.color);
            _states = states;
        }

        public void SetData(PlayerData data)
        {
            _data = data;
        }

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            int shrineCount = _data.EdificeCount(EdificeGroupId.Shrine), shrineMaxRes = _states.GetValue(PlayerStateId.ShrineMaxRes);

            _data.resources.AddAndClampToBlood(_states.GetValue(PlayerStateId.ShrinePassiveProfit) * shrineCount, shrineMaxRes);

            if (hexId == CONST.ID_GATE)
            {
                _data.resources.AddAndClampToBlood(_states.GetValue(PlayerStateId.ShrineProfit) * shrineCount, shrineMaxRes);
                _data.resources.ClampMain(_states.GetValue(PlayerStateId.MaxResources));
                return;
            }

            if (_states.IsMore(PlayerStateId.IsFreeGroundRes) && freeGroundRes != null)
                _data.resources.AddFrom(freeGroundRes);

            foreach (var crossroad in _data.Ports)
                _data.resources.AddFrom(crossroad.Profit(hexId, _states.GetValue(PlayerStateId.PortsRatioRes)));

            CurrenciesLite profit;
            foreach (var crossroad in _data.Urbans)
            {
                profit = crossroad.Profit(hexId);
                if (profit.Amount == 0 && crossroad.IsNotEnemy())
                    profit.RandomMainAdd(_states.GetValue(PlayerStateId.CompensationRes));
                _data.resources.AddFrom(profit);
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
            return (crossroad.GroupId != EdificeGroupId.None 
                                        || _states.IsMore(EdificeGroupId.ToIdAbility(upgradeGroup), _data.EdificeCount(upgradeGroup))) 
                                        && crossroad.CanUpgrade(_id);
        }
        public void CrossroadUpgradeBuy(Crossroad crossroad)
        {
            if (crossroad.UpgradeBuy(_id, out ACurrencies cost))
            {
                _data.EdificeAdd(crossroad);
                _data.resources.Pay(cost);
            }
        }

        public bool CanWallBuild(Crossroad crossroad) => _states.IsMore(PlayerStateId.IsWall) && crossroad.CanWallBuild(_id);
        public void CrossroadWallBuy(Crossroad crossroad)
        {
            if (crossroad.WallBuy(_id, out ACurrencies cost))
            {
                _data.EdificeAdd(crossroad);
                _data.resources.Pay(cost);
            }
                
        }

        public bool CanRoadBuild(Crossroad crossroad) => _states.IsMore(PlayerStateId.MaxRoads, _roads.Count) && crossroad.CanRoadBuild(_id);
        public bool CanRoadBuy() => _data.resources >= _roads.Cost;
        public void RoadBuy(Crossroad crossroad, CrossroadLink link)
        {
            link.SetStart(crossroad);
            _roads.BuildAndUnion(link);
            _data.resources.Pay(_roads.Cost);
        }

        public bool PerkBuy(IPerk<PlayerStateId> perk)
        {
            if (perk.TargetObject == TargetOfPerkId.Player && _states.TryAddPerk(perk))
            {
                _data.perks.Add(perk.Id);
                _data.resources.Pay(perk.Cost);
                return true;
            }

            return false;
        }

        public override string ToString() => $"Player: {_id}";


        //public void Load(Crossroads crossroads)
        //{
        //    if (_storage.TryGet(_id.ToString(), out PlayerLoadData data))
        //    {
        //        _resources = new(data.resources);
        //        CreateRoads(data);
        //        CreateCities(data);
        //        return;
        //    }

        //    #region Local: CreateRoads(), CreateCities(), CreateRoad(...)
        //    //=================================
        //    void CreateRoads(PlayerLoadData data)
        //    {
        //        if (data.roadsKey == null)
        //            return;

        //        foreach (var k in data.roadsKey)
        //            CreateRoad(k);
        //    }
        //    //=================================
        //    void CreateCities(PlayerLoadData data)
        //    {
        //        if (data.edifices == null)
        //            return;

        //        CrossroadLoadData loadData = new();
        //        Crossroad crossroad;
        //        for(int i = 0; i < EdificeGroupId.Count; i++)
        //        {
        //            foreach (var arr in data.edifices[i])
        //            {
        //                loadData.SetValues(arr);
        //                crossroad = crossroads.GetCrossroad(loadData.key);
        //                if (crossroad.Build(_id, loadData.id, loadData.isWall))
        //                    _edifices[i].Add(crossroad);
        //            }
        //        }

        //        _roads.SetRoadsEndings();
        //    }
        //    //=================================
        //    void CreateRoad(int[][] keys)
        //    {
        //        int count = keys.Length;
        //        if (count < 2) return;

        //        Key key = new(keys[0]);
        //        Crossroad start = crossroads.GetCrossroad(key);
        //        for (int i = 1; i < count; i++)
        //        {
        //            foreach (var link in start.Links)
        //            {
        //                if (link.Contains(key.SetValues(keys[i])))
        //                {
        //                    link.SetStart(start);
        //                    start = link.End;
        //                    _roads.Build(link);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    #endregion
        //}
    }
}
