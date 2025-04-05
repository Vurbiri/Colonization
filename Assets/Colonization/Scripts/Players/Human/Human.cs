//Assets\Colonization\Scripts\Players\Player\Human.cs
using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Characteristics.HumanAbilityId;

namespace Vurbiri.Colonization
{
    public partial class Human : IDisposable
    {
        #region Fields
        private readonly Coroutines _coroutines;

        private readonly Id<PlayerId> _id;
        private readonly Currencies _resources;
        private readonly ExchangeRate _exchange;
        private readonly PricesScriptable _prices;

        private readonly Edifices _edifices;
        private readonly Roads _roads;

        private readonly AbilitiesSet<HumanAbilityId> _abilities;
        private readonly Buffs _artefact;
        private readonly PerkTree _perks;

        private readonly WarriorsSpawner _spawner;
        private readonly ReactiveSet<Actor> _warriors = new(6);
        #endregion

        public ACurrenciesReactive Resources => _resources;
        public IReactive<CurrenciesLite> Exchange => _exchange;

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        public PerkTree Perks => _perks;

        public Human(Id<PlayerId> playerId, HumanStorage storage, Players.Settings settings, Hexagons hexagons)
        {
            _id = playerId;
            _coroutines = SceneContainer.Get<Coroutines>();

            var loadData = storage.LoadData;
            var visual = SceneContainer.Get<PlayersVisual>()[playerId];

            _perks = PerkTree.Create(settings, loadData);
            _abilities = settings.humanStates.Get(_perks);

            _roads = new(playerId, visual.color, settings.roadFactory, _coroutines);
            _prices = settings.prices;

            _resources = Currencies.Create(_abilities, _prices, loadData);
            _exchange = ExchangeRate.Create(_abilities, loadData);
            _artefact = Buffs.Create(settings.artefact.Settings, loadData);

            _spawner = new(new(playerId, _artefact, new(_perks)), settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

            if (loadData.isLoaded)
            {
                Crossroads crossroads = SceneContainer.Get<Crossroads>();

                _edifices = new(this, loadData.edifices, crossroads);
                storage.PopulateRoads(_roads, crossroads);

                for (int i = loadData.actors.Count - 1; i >= 0; i--)
                    _warriors.Add(_spawner.Load(loadData.actors[i], hexagons));
            }
            else
            {
                _edifices = new(_abilities);
            }

            bool instantGetValue = !loadData.isLoaded;
            storage.CurrenciesBind(_resources, instantGetValue);
            storage.ExchangeBind(_exchange, instantGetValue);
            storage.PerksBind(_perks, instantGetValue);
            storage.RoadsBind(_roads, instantGetValue);
            storage.ArtefactBind(_artefact, instantGetValue);
            storage.EdificesBind(_edifices.edifices, instantGetValue);
            storage.ActorsBind(_warriors);

            storage.LoadData = null;
        }

        public Ability GetAbility(Id<HumanAbilityId> id) => _abilities[id];

        public void EndTurn()
        {
            int countBuffs = 0;
            CurrenciesLite profit = new();
            foreach (var warrior in _warriors)
            {
                if (warrior.IsMainProfit)
                    profit.Increment(warrior.Hexagon.SurfaceId);
                if (warrior.IsAdvProfit)
                    countBuffs++;

                warrior.StatesUpdate();
            }

            _resources.AddFrom(profit);
            _artefact.Next(countBuffs);
        }
       
        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            _resources.AddBlood(_edifices.ShrinePassiveProfit);

            if (hexId == CONST.GATE_ID)
            {
                _resources.AddBlood(_edifices.ShrineProfit);
                _resources.ClampMain();
                return;
            }

            if (_abilities.IsTrue(IsFreeGroundRes) & freeGroundRes != null)
                _resources.AddFrom(freeGroundRes);

            _resources.AddFrom(_edifices.ProfitFromEdifices(hexId));
        }
        public void UpdateExchangeRate()
        {
            _exchange.Update();
        }

        public void BuyPerk(int typePerk, int idPerk)
        {
            if(_perks.TryAdd(typePerk, idPerk, out int cost))
                _resources.Add(CurrencyId.Mana, -cost);
        }
                
        public void Dispose()
        {
            _exchange.Dispose();
            _warriors.Dispose();
        }
    }
}
