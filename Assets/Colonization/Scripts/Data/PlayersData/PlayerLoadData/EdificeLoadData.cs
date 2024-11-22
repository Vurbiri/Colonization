//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\EdificeLoadData.cs
namespace Vurbiri.Colonization.Data
{
    public readonly struct EdificeLoadData
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
