//Assets\Colonization\Scripts\Data\HexagonsData\HexData.cs
namespace Vurbiri.Colonization.Data
{
    public class HexData
    {
        public readonly int id;
        public readonly int surfaceId;

        public HexData(int id, int surfaceId)
        {
            this.id = id;
            this.surfaceId = surfaceId;
        }
        public HexData(int[] array)
        {
            int i = 0;
            id = array[i++];
            surfaceId = array[i];
            //position = key.HexKeyToPosition();
        }

        public int[] ToArray() => new int[] { id, surfaceId };
    }
}
