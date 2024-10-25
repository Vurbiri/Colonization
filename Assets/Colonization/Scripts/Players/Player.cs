using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IValueId<PlayerId>
    {
        public Id<PlayerId> Id => _id;
        public PlayerVisual Visual { get; }
        public AReadOnlyCurrenciesReactive Resources => _data.resources;
        public IReactiveSubValues<int, CurrencyId> ExchangeRate => _exchangeRate;

        private readonly PlayerData _data;
        private readonly Id<PlayerId> _id;
        private readonly StatesSet<PlayerStateId> _states;
        private readonly Currencies _exchangeRate = new();
        private readonly Coroutines _coroutines;
        private Coroutine _recruiting;

        public Player(Id<PlayerId> playerId, PlayerVisual visual, StatesSet<PlayerStateId> states, PlayerData data)
        {
            _id = playerId;
            Visual = visual;
            _states = states;
            _data = data;

            _coroutines = SceneServices.Get<Coroutines>();
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
                _data.AddResourcesFrom(crossroad.ProfitFromPort(hexId, _states.GetValue(PlayerStateId.PortsRatioRes)));

            foreach (var crossroad in _data.Urbans)
                _data.AddResourcesFrom(crossroad.ProfitFromUrban(hexId, _states.GetValue(PlayerStateId.CompensationRes), _states.GetValue(PlayerStateId.WallDefence)));
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

        public bool CanAnyRecruitingWarriors(Crossroad crossroad) => _states.IsGreater(PlayerStateId.MaxWarrior, _data.WarriorsCount) && crossroad.CanRecruitingWarriors(_id);
        public bool CanRecruitingWarrior(Id<WarriorId> id) => _states.IsTrue(id.ToState());

        public void RecruitWarriors(Crossroad crossroad, Id<WarriorId> id) => _recruiting = _coroutines.Run(RecruitWarriors_Coroutine(crossroad, id));

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

        private IEnumerator RecruitWarriors_Coroutine(Crossroad crossroad, Id<WarriorId> id)
        {
            WaitResult<Hexagon> result = crossroad.GetHexagonForRecruiting_Wait();
            yield return result;

            if(result.Result == null)
                yield break;

            Debug.Log(result.Result.ToString());
        }

    }
}
