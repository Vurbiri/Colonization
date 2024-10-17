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
        [JsonProperty(P_CROSSROADS)]
        public readonly CrossroadsData crossroads;
        [JsonProperty(P_ROADS)]
        public readonly RoadsData roads;
        [JsonProperty(P_PERKS)]
        public readonly HashSet<int> perks;

        public IEnumerable<Crossroad> Ports => crossroads.values[EdificeGroupId.Port].Values;
        public IEnumerable<Crossroad> Urbans => crossroads.values[EdificeGroupId.Urban].Values;

        public PlayerData(ACurrencies currencies, Roads roads) 
        {
            resources = new(currencies);
            crossroads = new();
            this.roads = new(roads);
            perks = new();
        }

        internal PlayerData(int playerId, PlayerLoadData data, Crossroads crossroads, Roads roads)
        {
            this.resources = new(data.resources);
            this.crossroads = new(playerId, data.crossroads, crossroads);
            this.roads = new(data.roads, roads, crossroads);
            this.perks = new(data.perks);
        }


        public void EdificeAdd(Crossroad crossroad) => crossroads.values[crossroad.GroupId][crossroad.Key] = crossroad;
        public int EdificeCount(int edificeGroupId) => crossroads.values[edificeGroupId].Count;

    }
}
