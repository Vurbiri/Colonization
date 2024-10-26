using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    using Data;

    public partial class PlayerObjects
    {
        private readonly Currencies _resources;
        private readonly Edifices _edifices;
        private readonly Roads _roads;
        private readonly HashSet<int> _perks;

        private readonly PricesScriptable _prices;

        public AReadOnlyCurrenciesReactive Resources => _resources;

        public IEnumerable<Crossroad> Ports => _edifices.values[EdificeGroupId.Port].Values;
        public IEnumerable<Crossroad> Urbans => _edifices.values[EdificeGroupId.Urban].Values;

        public int RoadsCount => _roads.Count;
        public int WarriorsCount => 0;

        public PlayerObjects(PricesScriptable prices, PlayerData data, Roads roads)
        {
            _resources = new(prices.PlayersDefault);
            _edifices = new();
            _roads = roads;
            _perks = new();

            _prices = prices;

            Bind(data, true);
        }

        internal PlayerObjects(int playerId, PricesScriptable prices, PlayerData data, Crossroads crossroads, Roads roads)
        {
            _resources = new(data.Resources);
            _edifices = new(playerId, data.Edifices, crossroads);
            _roads = roads.Restoration(data.Roads, crossroads);
            _perks = new(data.Perks);

            _prices = prices;

            Bind(data, false);
        }

        public void AddResourcesFrom(ACurrencies other) => _resources.AddFrom(other);
        public void AddAndClampBlood(int value, int max) => _resources.AddAndClampBlood(value, max);
        public void ClampMainResources(int max) => _resources.ClampMain(max);

        public int GetEdificeCount(int edificeGroupId) => _edifices.values[edificeGroupId].Count;

        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            _resources.Pay(_prices.Edifices[crossroad.Id]);
            _edifices.Add(crossroad);
        }

        public void BuyWall()
        {
            _resources.Pay(_prices.Wall);
            _edifices.Signal();
        }

        public void BuyRoad(CrossroadLink link)
        {
            _resources.Pay(_prices.Road);
            _roads.BuildAndUnion(link);
        }

        private void Bind(PlayerData data, bool calling)
        {
            data.CurrenciesBind(_resources, calling);
            data.EdificesBind(_edifices, calling);
            data.RoadsBind(_roads, calling);
        }
    }
}
