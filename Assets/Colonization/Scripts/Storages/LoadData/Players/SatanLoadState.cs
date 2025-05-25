using Newtonsoft.Json;

namespace Vurbiri.Colonization.Storage
{
    [JsonConverter(typeof(Satan.Converter))]
    public class SatanLoadState
	{
        public readonly int level;
        public readonly int curse;
        public readonly int spawn;
        public readonly int maxDemons;

        public SatanLoadState()
        {
            maxDemons = CONST.DEFAULT_MAX_ACTORS;
        }

        public SatanLoadState(int level, int curse, int spawn, int maxDemons)
        {
            this.level = level;
            this.curse = curse;
            this.spawn = spawn;
            this.maxDemons = maxDemons;
        }
    }
}
