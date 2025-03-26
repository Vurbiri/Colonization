//Assets\Colonization\Scripts\Data\LoadData\EdificeLoadData.cs
namespace Vurbiri.Colonization.Data
{
    public class EdificeLoadData
	{
		public readonly Key key;
		public readonly int id;
        public readonly bool isWall;

		public EdificeLoadData(int[] data)
		{
			int i = 0;
			key = new(data[i++], data[i++]);
			id = data[i++];
            isWall = data[i++] > 0;
        }
    }
}
