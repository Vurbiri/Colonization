//Assets\Colonization\Scripts\Players\Player\Player.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Characteristics.PlayerAbilityId;

namespace Vurbiri.Colonization
{
    public partial class Player : IDisposable
    {
        private readonly Coroutines _coroutines;

        private readonly Id<PlayerId> _id;
        private readonly Currencies _resources;
        private readonly ExchangeRate _exchangeRate;
        private readonly PricesScriptable _prices;

        private readonly Edifices _edifices;
        private readonly Roads _roads;

        private readonly WarriorsSpawner _spawner;
        private readonly ListReactiveItems<Actor> _warriors = new();

        private readonly AbilitiesSet<PlayerAbilityId> _abilities;
        private readonly ReactiveList<IPerk> _perks;
       
        public ACurrenciesReactive Resources => _resources;
        //public IReactive<int, int> ExchangeRate => _exchangeRate;

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        public IReactiveList<IPerk> Perks => _perks;

        public Player(Id<PlayerId> playerId, PlayerSaveData data, Players.Settings settings)
        {
            _id = playerId;
            _coroutines = SceneServices.Get<Coroutines>();

            PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

            _abilities = settings.states;
            _roads = settings.roadsFactory.Create().Init(playerId, visual.color);

            _prices = settings.prices;
            _spawner = new(playerId, settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

            PlayerLoadData loadData = data.LoadData;
            if (loadData.isLoaded)
            {
                Crossroads crossroads = SceneObjects.Get<Crossroads>();
                Hexagons land = SceneObjects.Get<Hexagons>();

                _resources = new(loadData.resources, _abilities[MaxMainResources], _abilities[MaxBlood]);
                _edifices = new(playerId, loadData.edifices, crossroads, _abilities);
                _roads.Restoration(loadData.roads, crossroads);

                int count = loadData.warriors.Length;
                for (int i = 0; i < count; i++)
                    _warriors.Add(_spawner.Load(loadData.warriors[i], land));

                //_perks = new(data.Perks);
            }
            else
            {
                _resources = new(_prices.PlayersDefault, _abilities[MaxMainResources], _abilities[MaxBlood]);
                _edifices = new(_abilities);
                _perks = new();
            }

            _exchangeRate = new(_abilities);

            data.CurrenciesBind(_resources, !loadData.isLoaded);
            data.EdificesBind(_edifices.values);
            data.RoadsBind(_roads);
            data.WarriorsBind(_warriors);
        }

        public void UpdateExchangeRate()
        {
            _exchangeRate.Update();
        }

        public IReactive<int> GetAbilityReactive(Id<PlayerAbilityId> id) => _abilities[id];

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
            for (int i = _warriors.Count - 1; i >= 0; i--)
                _warriors[i].Dispose();
           
        }
    }
}
