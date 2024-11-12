using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    using Actors;
    using Data;
    using Reactive.Collections;

    public partial class PlayerObjects
    {
        private readonly Currencies _resources;
        private readonly Edifices _edifices;
        private readonly Roads _roads;
        private readonly ReactiveCollection<Actor> _warriors = new();
        private readonly HashSet<int> _perks;

        private readonly AbilitiesSet<PlayerAbilityId> _abilities;
        private readonly PricesScriptable _prices;
        private readonly WarriorsSpawner _spawner;

        public AReadOnlyCurrenciesReactive Resources => _resources;

        public Ability<PlayerAbilityId> ExchangeRate => _abilities.GetAbility(PlayerAbilityId.ExchangeRate);

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        public int WarriorsCount => _warriors.Count;

        public PlayerObjects(int playerId, bool isLoad, PlayerData data, Players.Settings settings)
        {
            PlayerVisual visual = SceneData.Get<PlayersVisual>()[playerId];

            _abilities = settings.states.GetAbilities();
            _roads = settings.roadsFactory.Create().Init(playerId, visual.color);
            _prices = settings.prices;
            _spawner = new(playerId, settings.warriorPrefab, visual.materialWarriors, settings.warriorsContainer);

            if (isLoad)
            {
                Crossroads crossroads = SceneObjects.Get<Crossroads>();
                Land land = SceneObjects.Get<Land>();

                _resources = new(data.Resources, _abilities.GetAbility(PlayerAbilityId.MaxMainResources), _abilities.GetAbility(PlayerAbilityId.MaxBlood));
                _roads.Restoration(data.Roads, crossroads);
                _edifices = new(playerId, data, crossroads);

                List<int[][]> warriorsData = data.Warriors;
                for (int i = 0; i < warriorsData.Count; i++)
                    _warriors.Add(_spawner.Create(warriorsData[i], land));

                _perks = new(data.Perks);
            }
            else
            {
                _resources = new(_prices.PlayersDefault, _abilities.GetAbility(PlayerAbilityId.MaxMainResources), _abilities.GetAbility(PlayerAbilityId.MaxBlood));
                _edifices = new();
                _perks = new();
            }

            data.CurrenciesBind(_resources, !isLoad);
            data.EdificesBind(_edifices.values, !isLoad);
            data.RoadsBind(_roads, !isLoad);
            data.WarriorsBind(_warriors, !isLoad);
        }
    }
}
