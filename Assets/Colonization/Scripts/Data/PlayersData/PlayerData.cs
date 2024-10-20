using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    using static JSON_KEYS;

    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerData
    {
        [JsonProperty(P_RESURSES)]
        public readonly Currencies resources;
        [JsonProperty(P_EDIFICES)]
        private readonly EdificesData _edifices;
        [JsonProperty(P_ROADS)]
        private readonly Roads _roads;
        [JsonProperty(P_PERKS)]
        private readonly HashSet<int> _perks;

        private readonly PricesScriptable _prices;

        public IEnumerable<Crossroad> Ports => _edifices.values[EdificeGroupId.Port].Values;
        public IEnumerable<Crossroad> Urbans => _edifices.values[EdificeGroupId.Urban].Values;

        public int RoadsCount => _roads.Count;
        public int WarriorsCount => 0;

        public PlayerData(PricesScriptable prices, Roads roads) 
        {
            resources = new(prices.PlayersDefault);
            _edifices = new();
            _roads = roads;
            _perks = new();

            _prices = prices;
        }

        internal PlayerData(int playerId, PricesScriptable prices, PlayerLoadData data, Crossroads crossroads, Roads roads)
        {
            resources = new(data.resources);
            _edifices = new(playerId, data.edifices, crossroads);
            _roads = roads.Restoration(data.roads, crossroads);
            _perks = new(data.perks);

            _prices = prices;
        }

        public void AddResourcesFrom(ACurrencies other) => resources.AddFrom(other);
        public void AddAndClampBlood(int value, int max) => resources.AddAndClampBlood(value, max);
        public void ClampMainResources(int max) => resources.ClampMain(max);

        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            _edifices.values[crossroad.GroupId][crossroad.Key] = crossroad;
            resources.Pay(_prices.Edifices[crossroad.Id]);
        }

        public void BuyWall()
        {
            resources.Pay(_prices.Wall);
        }

        public void BuyRoad(CrossroadLink link)
        {
            _roads.BuildAndUnion(link);
            resources.Pay(_prices.Road);
        }

        public int EdificeCount(int edificeGroupId) => _edifices.values[edificeGroupId].Count;

    }
}
