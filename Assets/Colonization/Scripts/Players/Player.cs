using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IValueId<PlayerId>
    {
        public Id<PlayerId> Id => _id;
        public PlayerVisual Visual { get; }
        public AReadOnlyCurrenciesReactive Resources { get; }
        public IReactiveSubValues<int, CurrencyId> ExchangeRate => _exchangeRate;

        private readonly PlayerObjects _obg;
        private readonly Id<PlayerId> _id;
        private readonly StatesSet<PlayerStateId> _states;
        private readonly Currencies _exchangeRate = new();
        private readonly Coroutines _coroutines;

        public Player(Id<PlayerId> playerId, PlayerVisual visual, StatesSet<PlayerStateId> states, PlayerObjects obj)
        {
            _id = playerId;
            _states = states;
            _obg = obj;
            Visual = visual;
            Resources = obj.Resources;
           
            _coroutines = SceneServices.Get<Coroutines>();
        }

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            int shrineCount = _obg.GetEdificeCount(EdificeGroupId.Shrine), shrineMaxRes = _states.GetValue(PlayerStateId.ShrineMaxRes);

            _obg.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrinePassiveProfit) * shrineCount, shrineMaxRes);

            if (hexId == CONST.ID_GATE)
            {
                _obg.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrineProfit) * shrineCount, shrineMaxRes);
                _obg.ClampMainResources(_states.GetValue(PlayerStateId.MaxResources));
                return;
            }

            if (_states.IsTrue(PlayerStateId.IsFreeGroundRes) && freeGroundRes != null)
                _obg.AddResourcesFrom(freeGroundRes);

            foreach (var crossroad in _obg.Ports)
                _obg.AddResourcesFrom(crossroad.ProfitFromPort(hexId, _states.GetValue(PlayerStateId.PortsRatioRes)));

            foreach (var crossroad in _obg.Urbans)
                _obg.AddResourcesFrom(crossroad.ProfitFromUrban(hexId, _states.GetValue(PlayerStateId.CompensationRes), _states.GetValue(PlayerStateId.WallDefence)));
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
                                        || _states.IsGreater(EdificeGroupId.ToIdAbility(upgradeGroup), _obg.GetEdificeCount(upgradeGroup))) 
                                        && crossroad.CanUpgrade(_id);
        }
        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            if (crossroad.BuyUpgrade(_id))
                _obg.BuyEdificeUpgrade(crossroad);
        }

        public bool CanAnyRecruitingWarriors(Crossroad crossroad) => _states.IsGreater(PlayerStateId.MaxWarrior, _obg.WarriorsCount) && crossroad.CanRecruitingWarriors(_id);
        public bool CanRecruitingWarrior(Id<WarriorId> id) => _states.IsTrue(id.ToState());

        public void RecruitWarriors(Crossroad crossroad, Id<WarriorId> id) => _coroutines.Run(RecruitWarriors_Coroutine(crossroad, id));

        public bool CanWallBuild(Crossroad crossroad) => _states.IsTrue(PlayerStateId.IsWall) && crossroad.CanWallBuild(_id);
        public void BuyWall(Crossroad crossroad)
        {
            if (crossroad.BuyWall(_id))
                _obg.BuyWall();
        }

        public bool CanRoadBuild(Crossroad crossroad) => _states.IsGreater(PlayerStateId.MaxRoads, _obg.RoadsCount) && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _obg.BuyRoad(crossroad.GetLinkAndSetStart(linkId));
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
