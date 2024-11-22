//Assets\Colonization\Scripts\Players\Player.cs
using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Player : IValueId<PlayerId>, IDisposable
    {
        protected readonly Objects _obj;
        protected readonly Currencies _exchangeRate = new();
        protected readonly Coroutines _coroutines;

        public Id<PlayerId> Id => _obj.id;
        public ACurrenciesReactive Resources => _obj.resources;
        public IReactive<int, int> ExchangeRate => _exchangeRate;

        public IReactiveList<Crossroad> Shrines => _obj.edifices.shrines;
        public IReactiveList<Crossroad> Ports => _obj.edifices.ports;
        public IReactiveList<Crossroad> Urbans => _obj.edifices.urbans;

        public Player(int playerId, bool isLoad, PlayerData data, Players.Settings settings)
        {
            _obj = new(playerId, isLoad, data, settings);

            _coroutines = SceneServices.Get<Coroutines>();
        }

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            _obj.ShrinePassiveProfit();

            if (hexId == CONST.ID_GATE)
            {
                _obj.GateAction();
                return;
            }

            _obj.ProfitFromEdifices(hexId, freeGroundRes);
        }

        public void UpdateExchangeRate()
        {
            Ability<PlayerAbilityId> exchangeRate = _obj.abilities.GetAbility(PlayerAbilityId.ExchangeRate);

            for (int i = 0; i < CurrencyId.CountMain; i++)
                _exchangeRate[i] = exchangeRate.NextValue;
        }

        public IReadOnlyReactive<int> GetAbilityReactive(Id<PlayerAbilityId> id) => _obj.GetAbilityReactive(id);

        public bool CanEdificeUpgrade(Crossroad crossroad) => _obj.CanEdificeUpgrade(crossroad) && crossroad.CanUpgrade(_obj.id);
        public void BuyEdificeUpgrade(Crossroad crossroad) => _obj.BuyEdificeUpgrade(crossroad);

        public bool CanAnyRecruitingWarriors(Crossroad crossroad) => _obj.IsNotMaxWarriors() && crossroad.CanRecruitingWarriors(_obj.id);
        public bool CanRecruitingWarrior(Id<WarriorId> id) => _obj.CanRecruitingWarrior(id);

        public void RecruitWarriors(Crossroad crossroad, Id<WarriorId> id) => _coroutines.Run(RecruitWarriors_Coroutine(crossroad, id));

        public bool CanWallBuild(Crossroad crossroad) => _obj.abilities.IsTrue(PlayerAbilityId.IsWall) && crossroad.CanWallBuild(_obj.id);
        public void BuyWall(Crossroad crossroad) => _obj.BuyWall(crossroad);

        public bool CanRoadBuild(Crossroad crossroad) => _obj.CanRoadBuild() && crossroad.CanRoadBuild(_obj.id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId) => _obj.BuyRoad(crossroad.GetLinkAndSetStart(linkId));

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

        public override string ToString() => $"Player: {_obj.id}";

        private IEnumerator RecruitWarriors_Coroutine(Crossroad crossroad, Id<WarriorId> id)
        {
            WaitResult<Hexagon> result = crossroad.GetHexagonForRecruiting_Wait();
            yield return result;

            if(result.Result == null)
                yield break;

            _obj.RecruitingWarrior(id.Value, result.Result);
        }

        public void Dispose()
        {
            _obj.Dispose();
            _exchangeRate.Dispose();
        }
    }
}
