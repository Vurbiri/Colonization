using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vurbiri.Colonization
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
        public readonly RoadData roads;
        [JsonProperty(P_PERKS)]
        public readonly HashSet<int> perks;

        public PlayerData(ACurrencies currencies) 
        {
            resources = new(currencies);
        }

        [JsonConstructor]
        public PlayerData(int[] resources, int[][] crossroads, int[] roads, HashSet<int> perks) 
        { 
            this.resources = new(resources);
            this.crossroads = new(crossroads);
            this.roads = new(roads);
            this.perks = perks;
        }
    }
}
