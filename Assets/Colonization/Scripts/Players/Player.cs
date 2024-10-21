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
        public Material MaterialWarriors => _visual.materialWarriors;
        public AReadOnlyCurrenciesReactive Resources => _data.resources;
        public IReactiveSubValues<int, CurrencyId> ExchangeRate => _exchangeRate;

        private PlayerData _data;
        private readonly Id<PlayerId> _id;
        private readonly PlayerVisual _visual;
        private readonly StatesSet<PlayerStateId> _states;
        private readonly Currencies _exchangeRate = new();

        public Player(Id<PlayerId> playerId, PlayerVisual visual, StatesSet<PlayerStateId> states)
        {
            _id = playerId;
            _visual = visual;
            _states = states;
        }

        public void SetData(PlayerData data)
        {
            _data = data;
        }

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            int shrineCount = _data.EdificeCount(EdificeGroupId.Shrine), shrineMaxRes = _states.GetValue(PlayerStateId.ShrineMaxRes);

            _data.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrinePassiveProfit) * shrineCount, shrineMaxRes);

            if (hexId == CONST.ID_GATE)
            {
                _data.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrineProfit) * shrineCount, shrineMaxRes);
                _data.ClampMainResources(_states.GetValue(PlayerStateId.MaxResources));
                return;
            }

            if (_states.IsTrue(PlayerStateId.IsFreeGroundRes) && freeGroundRes != null)
                _data.AddResourcesFrom(freeGroundRes);

            foreach (var crossroad in _data.Ports)
                _data.AddResourcesFrom(crossroad.Profit(hexId, _states.GetValue(PlayerStateId.PortsRatioRes)));

            CurrenciesLite profit;
            foreach (var crossroad in _data.Urbans)
            {
                profit = crossroad.Profit(hexId);
                if (profit.Amount == 0 && crossroad.IsNotEnemy())
                    profit.RandomMainAdd(_states.GetValue(PlayerStateId.CompensationRes));
                _data.AddResourcesFrom(profit);
            }
        }

        public void UpdateExchangeRate()
        {
            State<PlayerStateId> state = _states.GetState(PlayerStateId.ExchangeRate);

            for (int i = 0; i < CurrencyId.CountMain; i++)
                _exchangeRate.Set(i, state.NextValue);

        }

        public IReadOnlyReactiveValue<int> GetStateReactive(Id<PlayerStateId> id) => _states[id];

        public bool CanEdificeUpgrade(Crossroad crossroad)
        {
            int upgradeGroup = crossroad.NextGroupId;
            return (crossroad.GroupId != EdificeGroupId.None 
                                        || _states.IsGreater(EdificeGroupId.ToIdAbility(upgradeGroup), _data.EdificeCount(upgradeGroup))) 
                                        && crossroad.CanUpgrade(_id);
        }
        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            if (crossroad.BuyUpgrade(_id))
                _data.BuyEdificeUpgrade(crossroad);
        }

        public bool CanHiringWarriors(Crossroad crossroad) => _states.IsGreater(PlayerStateId.MaxWarrior, _data.WarriorsCount) && crossroad.CanHiringWarriors(_id);
        public void HireWarriors(int id, Crossroad crossroad) {}

        public bool CanWallBuild(Crossroad crossroad) => _states.IsTrue(PlayerStateId.IsWall) && crossroad.CanWallBuild(_id);
        public void BuyWall(Crossroad crossroad)
        {
            if (crossroad.BuyWall(_id))
                _data.BuyWall();
        }

        public bool CanRoadBuild(Crossroad crossroad) => _states.IsGreater(PlayerStateId.MaxRoads, _data.RoadsCount) && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _data.BuyRoad(crossroad.GetLinkAndSetStart(linkId));
        }

        public bool BuyPerk(IPerk<PlayerStateId> perk)
        {
            if (perk.TargetObject == TargetOfPerkId.Player && _states.TryAddPerk(perk))
            {
                Debug.LogWarning("Player PerkBuy");
                //_data.perks.Add(perk.Id);
                //_data.resources.Pay(perk.Cost);
                return true;
            }

            return false;
        }

        public override string ToString() => $"Player: {_id}";

    }
}
