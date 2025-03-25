//Assets\Colonization\Scripts\Players\Player\Human.cs
using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
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
        private readonly ExchangeRate _exchangeRate;
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
        //public IReactive<int, int> ExchangeRate => _exchangeRate;

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        public PerkTree Perks => _perks;

        public Human(Id<PlayerId> playerId, HumanSaveData data, Players.Settings settings, Hexagons hexagons)
        {
            _id = playerId;
            _coroutines = SceneServices.Get<Coroutines>();

            HumanLoadData loadData = data.LoadData;
            PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

            _perks = PerkTree.Create(settings, loadData);
            _abilities = settings.humanStates.Get(_perks);

            _roads = new(playerId, visual.color, settings.roadFactory, _coroutines);
            _prices = settings.prices;

            _resources = Currencies.Create(_abilities, _prices, loadData);
            _artefact = Buffs.Create(settings.artefact.Settings, loadData);

            _spawner = new(new(playerId, _artefact, new(_perks)), settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

            if (loadData.isLoaded)
            {
                Crossroads crossroads = SceneObjects.Get<Crossroads>();

                _edifices = new(playerId, loadData.edifices, crossroads, _abilities);
                _roads.Restoration(loadData.roads, crossroads);

                int count = loadData.actors.Length;
                for (int i = 0; i < count; i++)
                    _warriors.Add(_spawner.Load(loadData.actors[i], hexagons));
            }
            else
            {
                _edifices = new(_abilities);
            }

            _exchangeRate = new(_abilities);

            bool isNotLoaded = !loadData.isLoaded;
            data.CurrenciesBind(_resources, isNotLoaded);
            data.PerksBind(_perks, isNotLoaded);
            data.RoadsBind(_roads, isNotLoaded);
            data.ArtefactBind(_artefact, isNotLoaded);
            data.EdificesBind(_edifices.values);
            data.ActorsBind(_warriors);

            data.LoadData = null;
        }

        public IAbility GetAbility(Id<HumanAbilityId> id) => _abilities[id];

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
            _exchangeRate.Update();
        }

        public void BuyPerk(int typePerk, int idPerk)
        {
            if(_perks.TryAdd(typePerk, idPerk, out int cost))
                _resources.Add(CurrencyId.Mana, -cost);
        }
                
        public void Dispose()
        {
            _resources.Dispose();
            _perks.Dispose();
            _exchangeRate.Dispose();
            _edifices.Dispose();
            _roads.Dispose();
            _warriors.Dispose();
            _artefact.Dispose();
            _abilities.Dispose();
        }
    }
}
