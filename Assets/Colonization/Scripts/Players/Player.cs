namespace Vurbiri.Colonization
{
    using Actors;
    using Newtonsoft.Json;
    using Reactive;
    using System.Collections;
    using UnityEngine;

    [JsonObject(MemberSerialization.OptIn)]
    public class Player : IValueId<PlayerId>
    {
        private readonly PlayerObjects _obj;
        private readonly Id<PlayerId> _id;
        private readonly Currencies _exchangeRate = new();
        private readonly Coroutines _coroutines;

        public Id<PlayerId> Id => _id;
        public AReadOnlyCurrenciesReactive Resources { get; }
        public IReactiveSubValues<int, CurrencyId> ExchangeRate => _exchangeRate;

        public Player(Id<PlayerId> playerId, PlayerObjects obj)
        {
            _id = playerId;
            _obj = obj;
            Resources = obj.Resources;

            _coroutines = SceneServices.Get<Coroutines>();
        }

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            _obj.ShrinePassiveProfit();

            if (hexId == CONST.ID_GATE)
            {
                _obj.ShrineProfit();
                _obj.ClampMainResources();
                return;
            }

            _obj.ProfitFromEdifices(hexId, freeGroundRes);
        }

        public void UpdateExchangeRate()
        {
            Ability<PlayerAbilityId> exchangeRate = _obj.ExchangeRate;

            for (int i = 0; i < CurrencyId.CountMain; i++)
                _exchangeRate.Set(i, exchangeRate.NextValue());

        }

        public IReadOnlyReactiveValue<int> GetAbilityReactive(Id<PlayerAbilityId> id) => _obj.GetAbilityReactive(id);

        public bool CanEdificeUpgrade(Crossroad crossroad) => _obj.CanEdificeUpgrade(crossroad) && crossroad.CanUpgrade(_id);
        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            if (crossroad.BuyUpgrade(_id))
                _obj.BuyEdificeUpgrade(crossroad);
        }

        public bool CanAnyRecruitingWarriors(Crossroad crossroad) => _obj.IsNotMaxWarriors() && crossroad.CanRecruitingWarriors(_id);
        public bool CanRecruitingWarrior(Id<WarriorId> id) => _obj.CanRecruitingWarrior(id);

        public void RecruitWarriors(Crossroad crossroad, Id<WarriorId> id) => _coroutines.Run(RecruitWarriors_Coroutine(crossroad, id));

        public bool CanWallBuild(Crossroad crossroad) => _obj.CanWallBuild() && crossroad.CanWallBuild(_id);
        public void BuyWall(Crossroad crossroad)
        {
            if (crossroad.BuyWall(_id))
                _obj.BuyWall(crossroad);
        }

        public bool CanRoadBuild(Crossroad crossroad) => _obj.CanRoadBuild() && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _obj.BuyRoad(crossroad.GetLinkAndSetStart(linkId));
        }

        public bool BuyPerk(IPerkSettings perk)
        {
            Debug.LogWarning("Player PerkBuy");

            //if (perk.TargetObject == TargetOfPerkId.Player && _states.TryAddPerk(perk))
            //{
                
            //    _data.perks.Add(perk.Id);
            //    _data.resources.Pay(perk.Cost);
            //    return true;
            //}

            return false;
        }

        public override string ToString() => $"Player: {_id}";

        private IEnumerator RecruitWarriors_Coroutine(Crossroad crossroad, Id<WarriorId> id)
        {
            WaitResult<Hexagon> result = crossroad.GetHexagonForRecruiting_Wait();
            yield return result;

            if(result.Result == null)
                yield break;

            _obj.RecruitingWarrior(id.Value, result.Result);
        }

    }
}
