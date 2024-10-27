using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    using Data;
    using Reactive.Collections;

    public partial class PlayerObjects
    {
        private readonly Currencies _resources;
        private readonly Edifices _edifices;
        private readonly Roads _roads;
        private readonly HashSet<int> _perks;
        private readonly HashSet<Warrior> _warriors = new();

        private readonly StatesSet<PlayerStateId> _states;
        private readonly PricesScriptable _prices;
        private readonly WarriorsSpawner _spawner;

        public AReadOnlyCurrenciesReactive Resources => _resources;

        public State<PlayerStateId> ExchangeRate => _states.GetState(PlayerStateId.ExchangeRate);

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        public int WarriorsCount => 0;

        public PlayerObjects(int playerId, bool isLoad, PlayerData data, Players.Settings settings)
        {
            PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

            _states = settings.states.GetAbilities();
            _roads = settings.roadsFactory.Create().Init(playerId, visual.color);
            _prices = settings.prices;
            _spawner = new(playerId, settings.warriorPrefab, visual.materialWarriors, settings.warriorsContainer);

            if (isLoad)
            {
                Crossroads crossroads = SceneObjects.Get<Crossroads>();

                _resources = new(data.Resources);
                _edifices = new(playerId, data, crossroads);
                _roads.Restoration(data.Roads, crossroads);
                _perks = new(data.Perks);
            }
            else
            {
                _resources = new(_prices.PlayersDefault);
                _edifices = new();
                _perks = new();
            }

            data.CurrenciesBind(_resources, !isLoad);
            data.EdificesBind(_edifices.values, !isLoad);
            data.RoadsBind(_roads, !isLoad);
        }

        
    }
}
