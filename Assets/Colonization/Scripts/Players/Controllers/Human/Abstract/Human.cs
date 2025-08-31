using System;
using System.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.Characteristics.HumanAbilityId;

namespace Vurbiri.Colonization
{
    public abstract partial class Human : Player, IDisposable
    {
        #region Fields
        protected readonly Currencies _resources;
        protected readonly ExchangeRate _exchange;

        protected readonly Edifices _edifices;
        protected readonly Roads _roads;

        protected readonly SpellBook _spellBook;

        protected readonly ReadOnlyAbilities<HumanAbilityId> _abilities;
        protected readonly Artefact _artefact;
        protected readonly PerkTree _perks;

        protected readonly WarriorsSpawner _spawner;
        #endregion

        public Currencies Resources => _resources;
        public ExchangeRate Exchange => _exchange;

        public bool IsMaxWarriors => _abilities.IsLessOrEqual(MaxWarrior, Actors.Count);

        public Roads Roads => _roads;

        public Artefact Artefact => _artefact;
        public PerkTree Perks => _perks;
        public SpellBook SpellBook => _spellBook;

        public Human(int playerId, Settings settings) : base(playerId)
        {
            var storage = GameContainer.Storage.Humans[playerId];
            var loadData = storage.LoadData;

            _perks = PerkTree.Create(settings, loadData);
            _abilities = settings.humanAbilities.Get(_perks);

            _roads = new(playerId, settings.roadFactory);

            _resources = Currencies.Create(_abilities, GameContainer.Prices.HumanDefault, loadData);
            _exchange = ExchangeRate.Create(_abilities, loadData);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(new(playerId, new WarriorPerks(_perks), _artefact));

            if (loadData.isLoaded)
            {
                _edifices = new(this, loadData.edifices);
                storage.PopulateRoads(_roads, GameContainer.Crossroads);

                for (int i = loadData.actors.Count - 1; i >= 0; i--)
                    _spawner.Load(loadData.actors[i]);
            }
            else
            {
                _edifices = new(_abilities);
            }

            _spellBook = new(this);

            var balance = GameContainer.Balance;
            balance.BindShrines(_edifices.shrines);
            balance.BindBlood(_resources.Get(CurrencyId.Blood));

            bool instantGetValue = !loadData.isLoaded;
            storage.BindCurrencies(_resources, instantGetValue);
            storage.BindExchange(_exchange, instantGetValue);
            storage.BindPerks(_perks, instantGetValue);
            storage.BindRoads(_roads, instantGetValue);
            storage.BindArtefact(_artefact, instantGetValue);
            storage.BindEdifices(_edifices.edifices, instantGetValue);
            storage.BindActors(Actors);
            storage.LoadData = null;

            GameContainer.Crossroads.BindEdifices(_edifices.edifices, instantGetValue);
        }

        public Ability GetAbility(Id<HumanAbilityId> id) => _abilities[id];

        public ReadOnlyReactiveList<Crossroad> GetEdifices(Id<EdificeGroupId> id) => _edifices.edifices[id];

        public void BuyPerk(int typePerk, int idPerk)
        {
            if (_perks.TryAdd(typePerk, idPerk, out int cost))
            {
                _resources.RemoveBlood(cost);
                 
                if (typePerk == TypeOfPerksId.Economic | (idPerk >= EconomicPerksId.ExchangeSaleChance_1 & idPerk <= EconomicPerksId.ExchangeRate_1))
                    _exchange.Update();
            }
        }

        public void AddOrder(int order, CurrenciesLite cost)
        {
            if (order > 0)
            {
                GameContainer.Balance.Add(order);
                GameContainer.Score.ForAddingOrder(_id, order);
                _resources.Remove(cost);
            }
        }

        #region Resources
        public void AddResources(CurrenciesLite value) => _resources.Add(value);
        public bool IsPay(CurrenciesLite value) => _resources >= value;
        public void Pay(CurrenciesLite value) => _resources.Remove(value);
        #endregion

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

                _resources.Remove(GameContainer.Prices.Edifices[edificeId]);
                GameContainer.Score.ForBuilding(_id, edificeId);
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
        public WaitSignal BuyWall(Crossroad crossroad) => BuyWall(crossroad, GameContainer.Prices.Wall);
        public WaitSignal BuyWall(Crossroad crossroad, CurrenciesLite cost)
        {
            ReturnSignal returnSignal = crossroad.BuildWall(_id, true);
            if (returnSignal)
            {
                _resources.Remove(cost);
                _edifices.edifices[crossroad.GroupId].Signal(crossroad);
            }
            return returnSignal.signal;
        }
        #endregion

        #region Roads
        public bool CanRoadBuild(Crossroad crossroad) => _abilities.IsGreater(MaxRoad, _roads.Count) && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _resources.Remove(GameContainer.Prices.Road);
            _roads.BuildAndUnion(crossroad.GetLinkAndSetStart(linkId));
        }
        #endregion

        #region Warriors
        public bool CanAnyRecruiting(Crossroad crossroad)
        {
            return _abilities.IsGreater(MaxWarrior, Actors.Count) && crossroad.CanRecruiting(_id);
        }
        public bool CanRecruiting(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());

        public void Recruiting(Id<WarriorId> id, Crossroad crossroad) => Recruiting_Cn(id, crossroad).Start();
        public void Recruiting(Id<WarriorId> id, Hexagon hexagon) => Recruiting(id, hexagon, GameContainer.Prices.Warriors[id.Value]);
        public void Recruiting(Id<WarriorId> id, Hexagon hexagon, CurrenciesLite cost)
        {
            _resources.Remove(cost);
            Actor actor = _spawner.Create(id, hexagon);
            actor.IsPersonTurn = _isPerson;
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

        sealed override public void Dispose()
        {
            base.Dispose();
            _exchange.Dispose();
        }
    }
}
