using Newtonsoft.Json;

namespace Vurbiri.Colonization.Storage
{
    [JsonConverter(typeof(Satan.Converter))]
    public class SatanLoadState
	{
        public readonly int level;
        public readonly int curse;
        public readonly int spawn;

        public SatanLoadState() { }

        public SatanLoadState(int level, int curse, int spawn)
        {
            this.level = level;
            this.curse = curse;
            this.spawn = spawn;
        }
    }
}
