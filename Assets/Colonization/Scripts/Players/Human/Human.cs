//Assets\Colonization\Scripts\Players\Player\Human.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
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
        private readonly ExchangeRate _exchangeRate;
        private readonly PricesScriptable _prices;

        private readonly Edifices _edifices;
        private readonly Roads _roads;

        private readonly AbilitiesSet<HumanAbilityId> _abilities;
        private readonly Buffs _artefact;
        private readonly ReactiveList<IPerk> _perks;

        private readonly WarriorsSpawner _spawner;
        private readonly ListReactiveItems<Actor> _warriors = new();
        #endregion

        public ACurrenciesReactive Resources => _resources;
        //public IReactive<int, int> ExchangeRate => _exchangeRate;

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        public IReactiveList<IPerk> Perks => _perks;

        public Human(Id<PlayerId> playerId, HumanSaveData data, Players.Settings settings, Hexagons land)
        {
            _id = playerId;
            _coroutines = SceneServices.Get<Coroutines>();

            PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

            _abilities = settings.humanStates;
            _roads = new(playerId, visual.color, settings.roadFactory, _coroutines);
            _prices = settings.prices;

            HumanLoadData loadData = data.LoadData;
            bool isLoaded = loadData != null;
            if (isLoaded)
            {
                Crossroads crossroads = SceneObjects.Get<Crossroads>();

                _resources = new(loadData.resources, _abilities[MaxMainResources], _abilities[MaxBlood]);
                _edifices = new(playerId, loadData.edifices, crossroads, _abilities);
                _roads.Restoration(loadData.roads, crossroads);

                _artefact = new(settings.artefact.Settings, loadData.artefact);

                //_perks = new(data.Perks);

                _spawner = new(new(playerId, _artefact), settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

                int count = loadData.actors.Count;
                for (int i = 0; i < count; i++)
                    _warriors.Add(_spawner.Load(loadData.actors[i], land));

                data.LoadData = null;
            }
            else
            {
                _resources = new(_prices.PlayersDefault, _abilities[MaxMainResources], _abilities[MaxBlood]);
                _edifices = new(_abilities);
                _artefact = new(settings.artefact.Settings);
                _perks = new();

                _spawner = new(new(playerId, _artefact), settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);
            }

            _exchangeRate = new(_abilities);

            data.CurrenciesBind(_resources, !isLoaded);
            data.EdificesBind(_edifices.values);
            data.RoadsBind(_roads, !isLoaded);
            data.ArtefactBind(_artefact, !isLoaded);
            data.ActorsBind(_warriors);
        }

        public void UpdateExchangeRate()
        {
            _exchangeRate.Update();
        }

        public IReactive<int> GetAbilityReactive(Id<HumanAbilityId> id) => _abilities[id];

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

        
        public void Dispose()
        {
            _exchangeRate.Dispose();
            _edifices.Dispose();
            _roads.Dispose();
            for (int i = _warriors.Count - 1; i >= 0; i--)
                _warriors[i].Dispose();
           
        }
    }
}
