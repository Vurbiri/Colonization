using System;
using System.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Characteristics.HumanAbilityId;

namespace Vurbiri.Colonization
{
    public abstract partial class Human : IDisposable
    {
        #region Fields
        protected readonly Coroutines _coroutines;

        protected readonly int _id;
        protected readonly bool _isPlayer;
        protected readonly Currencies _resources;
        protected readonly ExchangeRate _exchange;
        protected readonly Prices _prices;

        protected readonly Balance _balance;
        protected readonly Score _score;

        protected readonly Edifices _edifices;
        protected readonly Roads _roads;

        protected readonly SpellBook _spellBook;

        protected readonly AbilitiesSet<HumanAbilityId> _abilities;
        protected readonly Artefact _artefact;
        protected readonly PerkTree _perks;

        protected readonly WarriorsSpawner _spawner;
        protected readonly ReactiveSet<Actor> _warriors = new(CONST.DEFAULT_MAX_ACTORS);
        protected readonly Unsubscription _unsubscriber;
        #endregion

        public int Id => _id;

        public ACurrenciesReactive Resources => _resources;
        public ExchangeRate Exchange => _exchange;

        public ReadOnlyReactiveSet<Actor> Warriors => _warriors;

        public Roads Roads => _roads;

        public Artefact Artefact => _artefact;
        public PerkTree Perks => _perks;

        public Human(int playerId, HumanStorage storage, Players.Settings settings)
        {
            _id = playerId;
            _isPlayer = playerId == PlayerId.Player;
            _balance = settings.balance;
            _score = settings.score;
            _coroutines = settings.coroutines;
            
            var loadData = storage.LoadData;
            var visual = SceneContainer.Get<HumansMaterials>()[playerId];

            _perks = PerkTree.Create(settings, loadData);
            _abilities = settings.humanStates.Get(_perks);

            _roads = new(playerId, visual.color, settings.roadFactory, _coroutines);
            _prices = settings.prices;

            _resources = Currencies.Create(_abilities, _prices, loadData);
            _exchange = ExchangeRate.Create(_abilities, loadData);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(new(playerId, _artefact, new(_perks)), settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

            if (loadData.isLoaded)
            {
                _edifices = new(this, loadData.edifices, settings.crossroads);
                storage.PopulateRoads(_roads, settings.crossroads);

                Warrior warrior;
                for (int i = loadData.actors.Count - 1; i >= 0; i--)
                {
                    warrior = _spawner.Load(loadData.actors[i], settings.hexagons);
                    warrior.OnKilled.Add(ActorKill);
                    _warriors.Add(warrior);
                }
            }
            else
            {
                _edifices = new(_abilities);
            }

            _spellBook = new(this);

            bool instantGetValue = !loadData.isLoaded;
            storage.BindCurrencies(_resources, instantGetValue);
            storage.BindExchange(_exchange, instantGetValue);
            storage.BindPerks(_perks, instantGetValue);
            storage.BindRoads(_roads, instantGetValue);
            storage.BindArtefact(_artefact, instantGetValue);
            storage.BindEdifices(_edifices.edifices, instantGetValue);
            storage.BindActors(_warriors);

            storage.LoadData = null;

            settings.crossroads.BindEdifices(_edifices.edifices, instantGetValue);

            settings.balance.BindShrines(_edifices.shrines);
            settings.balance.BindBlood(_resources.Get(CurrencyId.Blood));
        }

        public Ability GetAbility(Id<HumanAbilityId> id) => _abilities[id];

        public ReadOnlyReactiveList<Crossroad> GetEdifices(Id<EdificeGroupId> id) => _edifices.edifices[id];

        public void BuyOrder(int order, int price)
        {
            _balance.AddBalance(order);
            _score.OnAddOrder(_id, order);
            _resources.AddMain(CurrencyId.Mana, -price);
        }

        public void BuyCast(int type, int id, SpellParam param)
        {
            if(_spellBook.TryCast(type, id, param, out int cost))
                _resources.AddMain(CurrencyId.Mana, -cost);
        }

        public void BuyPerk(int typePerk, int idPerk)
        {
            if (_perks.TryAdd(typePerk, idPerk, out int cost))
            {
                _resources.PayInBlood(cost);
                 
                if (typePerk == TypeOfPerksId.Economic | (idPerk >= EconomicPerksId.ExchangeSaleChance_1 & idPerk <= EconomicPerksId.ExchangeRate_1))
                    _exchange.Update();
            }
        }

        public void AddResources(CurrenciesLite value) => _resources.Add(value);


        #region Edifice
        public bool CanEdificeUpgrade(Crossroad crossroad) => _edifices.CanEdificeUpgrade(crossroad) && crossroad.CanUpgrade(_id);
        public bool IsEdificeUnlock(Id<EdificeId> id) => _edifices.IsEdificeUnlock(id);
        public WaitSignal BuyEdificeUpgrade(Crossroad crossroad)
        {
            ReturnSignal returnSignal = crossroad.BuyUpgrade(_id);
            if (returnSignal)
            {
                int edificeId = crossroad.Id.Value;
                _edifices.edifices[crossroad.GroupId].AddOrChange(crossroad);

                _resources.Pay(_prices.Edifices[edificeId]);
                _score.OnBuild(_id, edificeId);
            }
            return returnSignal.signal;
        }

        public ReturnSignal BuildPort(Crossroad crossroad)
        {
            if (crossroad.NextGroupId == EdificeGroupId.Port)
            {
                ReturnSignal returnSignal = crossroad.BuyUpgrade(_id);
                if (returnSignal)
                    _edifices.edifices[crossroad.GroupId].Add(crossroad);
                return returnSignal;
            }
            return false;
        }

        public bool CanWallBuild(Crossroad crossroad) => crossroad.CanWallBuild(_id);
        public bool IsWallUnlock() => _abilities.IsTrue(IsWall);
        public WaitSignal BuyWall(Crossroad crossroad)
        {
            ReturnSignal returnSignal = crossroad.BuyWall(_id, _abilities[WallDefense], true);
            if (returnSignal)
            {
                _resources.Pay(_prices.Wall);
                _edifices.edifices[crossroad.GroupId].Signal(crossroad);
            }
            return returnSignal.signal;
        }
        #endregion

        #region Roads
        public bool CanRoadBuild(Crossroad crossroad) => _abilities.IsGreater(MaxRoad, _roads.Count) && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _resources.Pay(_prices.Road);
            _roads.BuildAndUnion(crossroad.GetLinkAndSetStart(linkId));
        }
        #endregion

        #region Warriors
        public bool CanAnyRecruiting(Crossroad crossroad)
        {
            return _abilities.IsGreater(MaxWarrior, _warriors.Count) && crossroad.CanRecruiting(_id);
        }
        public bool CanRecruiting(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());

        public void Recruiting(Id<WarriorId> id, Crossroad crossroad) => _coroutines.Run(Recruiting_Cn(id, crossroad));
        public void Recruiting(Id<WarriorId> id, Hexagon hexagon)
        {
            _resources.Pay(_prices.Warriors[id.Value]);

            Warrior warrior = _spawner.Create(id, hexagon);
            warrior.OnKilled.Add(ActorKill);
            warrior.IsPlayerTurn = _isPlayer;
            
            _warriors.Add(warrior);
        }

        protected IEnumerator Recruiting_Cn(Id<WarriorId> id, Crossroad crossroad)
        {
            WaitResult<Hexagon> result = crossroad.GetHexagonForRecruiting_Wait();
            yield return result;

            if (result.Value == null)
                yield break;

            Recruiting(id, result.Value);
        }
        #endregion

        public void Dispose()
        {
            _unsubscriber.Unsubscribe();
            _exchange.Dispose();
            _warriors.Dispose();
        }

        protected void ActorKill(Id<PlayerId> target, int actorId)
        {
            if (target == PlayerId.Satan)
            {
                _score.OnDemonKill(_id, actorId);
                _resources.AddBlood(actorId + 1);
            }
            else if (target != _id)
            {
                _score.OnWarriorKill(_id, actorId);
            }
        }
    }
}
