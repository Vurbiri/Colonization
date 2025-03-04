//Assets\Colonization\Scripts\Players\Player\Player.cs
using System;
using System.Collections.Generic;
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
        protected readonly Coroutines _coroutines;

        protected readonly Id<PlayerId> _id;
        protected readonly Currencies _resources;
        protected readonly ExchangeRate _exchangeRate;
        protected readonly PricesScriptable _prices;

        protected readonly Edifices _edifices;
        protected readonly Roads _roads;

        protected readonly WarriorsSpawner _spawner;
        protected readonly ListReactiveItems<Actor> _warriors = new();

        protected readonly AbilitiesSet<PlayerAbilityId> _abilities;
        protected readonly HashSet<int> _perks;
       
        public ACurrenciesReactive Resources => _resources;
        public IReactive<int, int> ExchangeRate => _exchangeRate;

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        #region Constructor
        public Player(Id<PlayerId> playerId, PlayerSaveData data, Players.Settings settings)
        {
            _coroutines = SceneServices.Get<Coroutines>();

            _id = playerId;

            PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

            _abilities = settings.states;
            _roads = settings.roadsFactory.Create().Init(playerId, visual.color);

            _prices = settings.prices;
            _spawner = new(playerId, settings.warriorPrefab, visual.materialWarriors, settings.actorsContainer);

            PlayerLoadData loadData = data.LoadData;

            if (loadData.isLoaded)
            {
                
                Crossroads crossroads = SceneObjects.Get<Crossroads>();
                Land land = SceneObjects.Get<Land>();

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
        #endregion

        public void UpdateExchangeRate()
        {
            _exchangeRate.Update();
        }

        public IReactive<int> GetAbilityReactive(Id<PlayerAbilityId> id) => _abilities[id];

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            _resources.AddBlood(_edifices.ShrinePassiveProfit);

            if (hexId == CONST.ID_GATE)
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
