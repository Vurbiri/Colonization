using Newtonsoft.Json;

namespace Vurbiri.Colonization.Data
{
    using static JSON_KEYS;

    internal class PlayerLoadData
    {
        [JsonProperty(P_RESURSES)]
        public int[] resources;
        [JsonProperty(P_CROSSROADS)]
        public int[][] crossroads;
        [JsonProperty(P_ROADS)]
        public int[][][] roads;
        [JsonProperty(P_PERKS)]
        public int[] perks;
    }
}
