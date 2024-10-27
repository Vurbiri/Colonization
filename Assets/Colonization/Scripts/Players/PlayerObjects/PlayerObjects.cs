using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    using Data;
    using Vurbiri.Reactive.Collections;

    public partial class PlayerObjects
    {
        private readonly Currencies _resources;
        private readonly Edifices _edifices;
        private readonly Roads _roads;
        private readonly HashSet<int> _perks;

        private readonly StatesSet<PlayerStateId> _states;
        private readonly PricesScriptable _prices;

        public AReadOnlyCurrenciesReactive Resources => _resources;

        public IReactiveList<Crossroad> Shrines => _edifices.shrines;
        public IReactiveList<Crossroad> Ports => _edifices.ports;
        public IReactiveList<Crossroad> Urbans => _edifices.urbans;

        public int WarriorsCount => 0;

        public PlayerObjects(int playerId, bool isLoad, PlayerData data, Players.Settings settings)
        {
            _states = settings.states.GetAbilities();
            _roads = settings.roadsFactory.Create().Init(playerId);
            _prices = settings.prices;

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

        public void ShrinePassiveProfit()
         => _resources.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrinePassiveProfit) * _edifices.shrines.Count, _states.GetValue(PlayerStateId.ShrineMaxRes));
        public void ShrineProfit()
         => _resources.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrineProfit) * _edifices.shrines.Count, _states.GetValue(PlayerStateId.ShrineMaxRes));

        public void ClampMainResources() => _resources.ClampMain(_states.GetValue(PlayerStateId.MaxResources));
        public void ProfitFromEdifices(int hexId, ACurrencies freeGroundRes)
        {
            if (_states.IsTrue(PlayerStateId.IsFreeGroundRes) && freeGroundRes != null)
                _resources.AddFrom(freeGroundRes);

            foreach (var crossroad in _edifices.ports)
                _resources.AddFrom(crossroad.ProfitFromPort(hexId, _states.GetValue(PlayerStateId.PortsRatioRes)));

            foreach (var crossroad in _edifices.urbans)
                _resources.AddFrom(crossroad.ProfitFromUrban(hexId, _states.GetValue(PlayerStateId.CompensationRes), _states.GetValue(PlayerStateId.WallDefence)));
        }

        public void AddResourcesFrom(ACurrencies other) => _resources.AddFrom(other);
        public void AddAndClampBlood(int value, int max) => _resources.AddAndClampBlood(value, max);
        

        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            _resources.Pay(_prices.Edifices[crossroad.Id]);
            _edifices.values[crossroad.GroupId].TryAdd(crossroad);
        }

        public void BuyWall(Crossroad crossroad)
        {
            _resources.Pay(_prices.Wall);
            _edifices.values[crossroad.GroupId].ChangeSignal(crossroad);
        }

        public void BuyRoad(CrossroadLink link)
        {
            _resources.Pay(_prices.Road);
            _roads.BuildAndUnion(link);
        }
    }
}
