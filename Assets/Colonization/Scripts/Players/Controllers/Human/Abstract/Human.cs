using System;
using System.Collections;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.HumanAbilityId;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Human : Player, IDisposable
    {
        #region ================== Fields ============================
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

        #region ================== Properties ============================
        public Currencies Resources  { [Impl(256)] get => _resources; }
        public ExchangeRate ExchangeRate { [Impl(256)] get => _exchange; }

        public bool IsMaxWarriors { [Impl(256)] get => _abilities.IsLessOrEqual(MaxWarrior, Actors.Count); }

        public ReadOnlyReactiveList<Crossroad> Ports    { [Impl(256)] get => _edifices.ports; }
        public ReadOnlyReactiveList<Crossroad> Colonies { [Impl(256)] get => _edifices.colonies; }
        public ReadOnlyReactiveList<Crossroad> Shrines  { [Impl(256)] get => _edifices.shrines; }

        public Roads Roads { [Impl(256)] get => _roads; }

        public ReadOnlyAbilities<HumanAbilityId> Abilities { [Impl(256)] get => _abilities; }
        public Artefact Artefact { [Impl(256)] get => _artefact; }
        public PerkTree Perks { [Impl(256)] get => _perks; }
        public SpellBook SpellBook { [Impl(256)] get => _spellBook; }
        #endregion

        public Human(int playerId, Settings settings, bool isPerson) : base(playerId, isPerson)
        {
            var storage = GameContainer.Storage.Humans[playerId];
            var loadData = storage.LoadData;

            _perks = PerkTree.Create(settings, loadData);
            _abilities = settings.humanAbilities.Get(_perks);

            _roads = new(playerId, settings.roadFactory);

            _resources = Currencies.Create(_abilities, GameContainer.Prices.HumanDefault, loadData);
            _exchange = ExchangeRate.Create(_abilities, _perks, loadData);
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
        
        [Impl(256)] public Ability GetAbility(Id<HumanAbilityId> id) => _abilities[id];

        [Impl(256)] public ReadOnlyReactiveList<Crossroad> GetEdifices(Id<EdificeGroupId> id) => _edifices.edifices[id];

        #region ================== Perks ============================
        [Impl(256)] public bool CanLearnPerk(int typePerk, int idPerk, int cost) => _resources[CurrencyId.Blood] >= cost & !_perks.IsPerkLearned(typePerk, idPerk);
        [Impl(256)] public void BuyPerk(Perk perk)
        {
            _perks.Learn(perk);
            _resources.RemoveBlood(perk.Cost);
        }
        [Impl(256)] public void BuyPerk(int typePerk, int idPerk, int cost)
        {
            _perks.Learn(typePerk, idPerk);
            _resources.RemoveBlood(cost);
        }
        #endregion

        public void AddOrder(int order, ReadOnlyMainCurrencies cost)
        {
            if (order > 0)
            {
                GameContainer.Chaos.Add(-order);
                GameContainer.Score.ForAddingOrder(_id, order);
                _resources.Remove(cost);
            }
        }

        #region ================== Resources ============================
        [Impl(256)] public bool IsPay(ReadOnlyMainCurrencies value) => _resources >= value;
        [Impl(256)] public void Pay(ReadOnlyMainCurrencies value) => _resources.Remove(value);
        #endregion

        #region ================== Edifice =============================
        [Impl(256)] public bool CanEdificeUpgrade(Crossroad crossroad) => _edifices.CanEdificeUpgrade(crossroad) && crossroad.CanUpgrade(_id);
        [Impl(256)] public bool IsEdificeUnlock(Id<EdificeId> id) => _edifices.IsEdificeUnlock(id);
        [Impl(256)] public void BuyEdificeUpgrade(Crossroad crossroad) => BuyEdificeUpgrade(crossroad, GameContainer.Prices.Edifices[crossroad.NextId]);
        public WaitSignal BuyEdificeUpgrade(Crossroad crossroad, ReadOnlyMainCurrencies cost)
        {
            ReturnSignal returnSignal = crossroad.BuyUpgrade(_id);
            if (returnSignal)
            {
                _edifices.edifices[crossroad.GroupId].AddOrReplace(crossroad);
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

        #region ================== Roads ===============================
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

        #region ================== Warriors ============================
        [Impl(256)] public bool CanAnyRecruiting(Crossroad crossroad)
        {
            return _abilities.IsGreater(MaxWarrior, Actors.Count) && crossroad.CanRecruiting(_id);
        }
        [Impl(256)] public bool CanRecruiting(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());

        [Impl(256)] public void Recruiting(Id<WarriorId> id, Hexagon hexagon) => _spawner.Create(id, hexagon);
        [Impl(256)] public void Recruiting(Id<WarriorId> id, Crossroad crossroad) => StartCoroutine(Recruiting_Cn(id, crossroad));
        public WaitSignal Recruiting_Wait(Id<WarriorId> id, Hexagon hexagon, ReadOnlyMainCurrencies cost)
        {
            WaitSignal signal = new();

            _resources.Remove(cost);

            var actor = _spawner.Create(id, hexagon);
            actor.Skin.EventStart.Add(signal.Send);

            return signal;
        }

        protected IEnumerator Recruiting_Cn(Id<WarriorId> id, Crossroad crossroad)
        {
            var hexagon = crossroad.GetHexagonForRecruiting_Wait();
            yield return hexagon;
            if (hexagon.IsNotNull)
                yield return Recruiting_Wait(id, hexagon, GameContainer.Prices.Warriors[id]);
        }
        #endregion
    }
}
