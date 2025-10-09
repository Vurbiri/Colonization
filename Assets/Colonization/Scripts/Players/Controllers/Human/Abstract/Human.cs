using System;
using System.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Characteristics.HumanAbilityId;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Human : Player, IDisposable
    {
        #region Fields
        protected readonly Currencies _resources;
        protected readonly ExchangeRate _exchange;

        protected readonly Edifices _edifices;
        protected readonly Roads _roads;

        protected readonly SpellBook _spellBook = new();

        protected readonly AbilitiesSet<HumanAbilityId> _abilities;
        protected readonly Artefact _artefact;
        protected readonly PerkTree _perks;

        protected readonly WarriorsSpawner _spawner;
        #endregion

        public Currencies Resources  { [Impl(256)] get => _resources; }
        public ExchangeRate ExchangeRate { [Impl(256)] get => _exchange; }

        public bool IsMaxWarriors { [Impl(256)] get => _abilities.IsLessOrEqual(MaxWarrior, Actors.Count); }

        public ReadOnlyReactiveList<Crossroad> Ports    { [Impl(256)] get => _edifices.ports; }
        public ReadOnlyReactiveList<Crossroad> Colonies { [Impl(256)] get => _edifices.colonies; }
        public ReadOnlyReactiveList<Crossroad> Shrines  { [Impl(256)] get => _edifices.shrines; }

        public Roads Roads { [Impl(256)] get => _roads; }

        public Artefact Artefact { [Impl(256)] get => _artefact; }
        public PerkTree Perks { [Impl(256)] get => _perks; }
        public SpellBook SpellBook { [Impl(256)] get => _spellBook; }

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

            var balance = GameContainer.Chaos;
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
        }

        // TSET !!!!!!!!!!!!!!
        public void SpawnTest(int id, int count)
        {
            UnityEngine.Debug.Log("SpawnTest");
            Hexagon hexagon;
            for (int i = 0; i < count; i++)
            {
                while (!(hexagon = GameContainer.Hexagons[HEX.NEARS.Random]).CanWarriorEnter) ;
                Actor actor = _spawner.Create(id, hexagon);
                actor.IsPersonTurn = _isPerson;
            }
        }
        public void SpawnTest(Id<WarriorId> id, Key key)
        {
            UnityEngine.Debug.Log("SpawnTest");
            Hexagon hexagon;
            if ((hexagon = GameContainer.Hexagons[key]).CanWarriorEnter)
            {
                Actor actor = _spawner.Create(id, hexagon);
                actor.IsPersonTurn = _isPerson;
            }
        }
        public void SpawnDemonTest(Id<DemonId> id, Key key)
        {
            UnityEngine.Debug.Log("SpawnDemonTest");
            Hexagon hexagon;
            if ((hexagon = GameContainer.Hexagons[key]).CanDemonEnter)
            {
                Actor actor = _spawner.CreateDemon(id, hexagon);
                actor.IsPersonTurn = _isPerson;
            }
        }
        public void SpawnDemonTest(int id, int count)
        {
            UnityEngine.Debug.Log("SpawnDemonTest");
            Hexagon hexagon;
            for (int i = 0; i < count; i++)
            {
                while (!(hexagon = GameContainer.Hexagons[HEX.NEARS.Random]).CanDemonEnter) ;
                Actor actor = _spawner.CreateDemon(id, hexagon);
                actor.IsPersonTurn = _isPerson;
            }
        }

        [Impl(256)] public Ability GetAbility(Id<HumanAbilityId> id) => _abilities[id];

        [Impl(256)] public ReadOnlyReactiveList<Crossroad> GetEdifices(Id<EdificeGroupId> id) => _edifices.edifices[id];

        public void BuyPerk(int typePerk, int idPerk)
        {
            if (_perks.TryAdd(typePerk, idPerk, out int cost))
            {
                _resources.RemoveBlood(cost);
                 
                if (typePerk == TypeOfPerksId.Economic | (idPerk >= EconomicPerksId.ExchangeSaleChance_1 & idPerk <= EconomicPerksId.ExchangeRate_1))
                    _exchange.Update();
            }
        }

        public void AddOrder(int order, ReadOnlyMainCurrencies cost)
        {
            if (order > 0)
            {
                GameContainer.Chaos.Add(-order);
                GameContainer.Score.ForAddingOrder(_id, order);
                _resources.Remove(cost);
            }
        }

        #region Resources
        [Impl(256)] public void AddResources(ReadOnlyMainCurrencies value) => _resources.Add(value);
        [Impl(256)] public bool IsPay(ReadOnlyMainCurrencies value) => _resources >= value;
        [Impl(256)] public void Pay(ReadOnlyMainCurrencies value) => _resources.Remove(value);
        #endregion

        #region Edifice
        [Impl(256)] public bool CanEdificeUpgrade(Crossroad crossroad) => _edifices.CanEdificeUpgrade(crossroad) && crossroad.CanUpgrade(_id);
        [Impl(256)] public bool IsEdificeUnlock(Id<EdificeId> id) => _edifices.IsEdificeUnlock(id);
        [Impl(256)] public void BuyEdificeUpgrade(Crossroad crossroad) => BuyEdificeUpgrade(crossroad, GameContainer.Prices.Edifices[crossroad.NextId]);
        public WaitSignal BuyEdificeUpgrade(Crossroad crossroad, ReadOnlyMainCurrencies cost)
        {
            ReturnSignal returnSignal = crossroad.BuyUpgrade(_id);
            if (returnSignal)
            {
                _edifices.edifices[crossroad.GroupId].AddOrChange(crossroad);
                _resources.Remove(cost);
                GameContainer.Score.ForBuilding(_id, crossroad.Id);

                if(crossroad.Id == EdificeId.Shrine)
                    s_shrinesCount.Increment();
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

        [Impl(256)] public bool CanWallBuild(Crossroad crossroad) => crossroad.CanWallBuild(_id);
        [Impl(256)] public bool IsWallUnlock() => _abilities.IsTrue(IsWall);
        [Impl(256)] public void BuyWall(Crossroad crossroad) => BuyWall(crossroad, GameContainer.Prices.Wall);
        public WaitSignal BuyWall(Crossroad crossroad, ReadOnlyMainCurrencies cost)
        {
            var returnSignal = crossroad.BuildWall(_id, true);
            if (returnSignal)
            {
                _resources.Remove(cost);
                GameContainer.Score.ForWall(_id);
                _edifices.edifices[crossroad.GroupId].Signal(crossroad);
            }
            return returnSignal.signal;
        }
        #endregion

        #region Roads
        [Impl(256)] public bool CanRoadBuild(Crossroad crossroad) => _abilities.IsGreater(MaxRoad, _roads.Count) && crossroad.CanRoadBuild(_id);
        [Impl(256)] public void BuyRoad(Crossroad crossroad, Id<LinkId> linkId) => BuyRoad(crossroad.Type, crossroad.Links[linkId], GameContainer.Prices.Road);
        public WaitSignal BuyRoad(Id<CrossroadType> startType, CrossroadLink link, ReadOnlyMainCurrencies cost)
        {
            var returnSignal = _roads.BuildAndUnion(startType, link);
            if (returnSignal)
            {
                _resources.Remove(cost);
                GameContainer.Score.ForRoad(_id);
            }
            return returnSignal.signal;
        }
        #endregion

        #region Warriors
        [Impl(256)] public bool CanAnyRecruiting(Crossroad crossroad)
        {
            return _abilities.IsGreater(MaxWarrior, Actors.Count) && crossroad.CanRecruiting(_id);
        }
        [Impl(256)] public bool CanRecruiting(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());

        [Impl(256)] public void Recruiting(Id<WarriorId> id, Crossroad crossroad) => Recruiting_Cn(id, crossroad).Start();
        [Impl(256)] public void Recruiting(Id<WarriorId> id, Hexagon hexagon) => Recruiting(id, hexagon, GameContainer.Prices.Warriors[id.Value]);
        public void Recruiting(Id<WarriorId> id, Hexagon hexagon, ReadOnlyMainCurrencies cost)
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

        sealed public override void Dispose()
        {
            base.Dispose();
            _exchange.Dispose();
        }
    }
}
