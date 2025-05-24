using Newtonsoft.Json;

namespace Vurbiri.Colonization.Storage
{
    [JsonConverter(typeof(Crossroad.Converter))]
    public class EdificeLoadData
	{
		public readonly Key key;
		public readonly int id;
        public readonly bool isWall;

        public EdificeLoadData(Key key, int id, bool isWall)
        {
            this.key = key;
            this.id = id;
            this.isWall = isWall;
        }
    }
}
