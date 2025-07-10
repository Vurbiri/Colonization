using System;
using System.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
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

        protected readonly AbilitiesSet<HumanAbilityId> _abilities;
        protected readonly Artefact _artefact;
        protected readonly PerkTree _perks;

        protected readonly WarriorsSpawner _spawner;
        protected readonly ReactiveSet<Actor> _actors = new(CONST.DEFAULT_MAX_ACTORS);
        protected readonly Unsubscription _unsubscriber;
        #endregion

        public int Id => _id;

        public ACurrenciesReactive Resources => _resources;
        public ExchangeRate Exchange => _exchange;

        public ReadOnlyReactiveSet<Actor> Warriors => _actors;
        public bool IsMaxWarriors => _abilities.IsGreater(MaxWarrior, _actors.Count);

        public Roads Roads => _roads;

        public Artefact Artefact => _artefact;
        public PerkTree Perks => _perks;

        public Human(int playerId, Settings settings) : base(playerId)
        {
            var storage = settings.storage.Humans[playerId];
            
            var loadData = storage.LoadData;
            var visual = SceneContainer.Get<HumansMaterials>()[playerId];

            _perks = PerkTree.Create(settings, loadData);
            _abilities = settings.humanAbilities.Get(_perks);

            _roads = new(playerId, visual.color, settings.roadFactory, s_coroutines);

            _resources = Currencies.Create(_abilities, s_states.prices.HumanDefault, loadData);
            _exchange = ExchangeRate.Create(_abilities, loadData);
            _artefact = Artefact.Create(settings.artefact, loadData);

            _spawner = new(new(playerId, new(_perks), _artefact), settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

            if (loadData.isLoaded)
            {
                _edifices = new(this, loadData.edifices, s_crossroads);
                storage.PopulateRoads(_roads, s_crossroads);

                for (int i = loadData.actors.Count - 1; i >= 0; i--)
                    _actors.Add(_spawner.Load(loadData.actors[i], s_hexagons));
            }
            else
            {
                _edifices = new(_abilities);
            }

            _spellBook = new(this);

            s_states.balance.BindWarriors(_actors);
            s_states.balance.BindShrines(_edifices.shrines);
            s_states.balance.BindBlood(_resources.Get(CurrencyId.Blood));

            bool instantGetValue = !loadData.isLoaded;
            storage.BindCurrencies(_resources, instantGetValue);
            storage.BindExchange(_exchange, instantGetValue);
            storage.BindPerks(_perks, instantGetValue);
            storage.BindRoads(_roads, instantGetValue);
            storage.BindArtefact(_artefact, instantGetValue);
            storage.BindEdifices(_edifices.edifices, instantGetValue);
            storage.BindActors(_actors);
            storage.LoadData = null;

            s_crossroads.BindEdifices(_edifices.edifices, instantGetValue);
        }

        public Ability GetAbility(Id<HumanAbilityId> id) => _abilities[id];

        public ReadOnlyReactiveList<Crossroad> GetEdifices(Id<EdificeGroupId> id) => _edifices.edifices[id];

        public void BuyCast(int type, int id, SpellParam param)
        {
            param.playerId = _id;
            _resources.Add(_spellBook.Cast(type, id, _resources[CurrencyId.Mana], param));
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

                _resources.Pay(s_states.prices.Edifices[edificeId]);
                s_states.score.ForBuilding(_id, edificeId);
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
            ReturnSignal returnSignal = crossroad.BuildWall(_id, true);
            if (returnSignal)
            {
                _resources.Pay(s_states.prices.Wall);
                _edifices.edifices[crossroad.GroupId].Signal(crossroad);
            }
            return returnSignal.signal;
        }
        #endregion

        #region Roads
        public bool CanRoadBuild(Crossroad crossroad) => _abilities.IsGreater(MaxRoad, _roads.Count) && crossroad.CanRoadBuild(_id);
        public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId)
        {
            _resources.Pay(s_states.prices.Road);
            _roads.BuildAndUnion(crossroad.GetLinkAndSetStart(linkId));
        }
        #endregion

        #region Warriors
        public bool CanAnyRecruiting(Crossroad crossroad)
        {
            return _abilities.IsGreater(MaxWarrior, _actors.Count) && crossroad.CanRecruiting(_id);
        }
        public bool CanRecruiting(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());

        public void Recruiting(Id<WarriorId> id, Crossroad crossroad) => s_coroutines.Run(Recruiting_Cn(id, crossroad));
        public void Recruiting(Id<WarriorId> id, Hexagon hexagon)
        {
            _resources.Pay(s_states.prices.Warriors[id.Value]);
            RecruitingFree(id, hexagon);
        }
        public void RecruitingFree(Id<WarriorId> id, Hexagon hexagon)
        {
            Warrior warrior = _spawner.Create(id, hexagon);
            warrior.IsPersonTurn = _isPerson;

            _actors.Add(warrior);
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
            _actors.Dispose();
        }
    }
}
