using Newtonsoft.Json;

namespace Vurbiri.Colonization.Storage
{
    [JsonConverter(typeof(Hexagon.Converter))]
    public readonly struct HexLoadData
	{
		public readonly int id;
		public readonly int surfaceId;

		public HexLoadData(int id, int surfaceId)
		{
			this.id = id;
			this.surfaceId = surfaceId;
		}
    }
}
