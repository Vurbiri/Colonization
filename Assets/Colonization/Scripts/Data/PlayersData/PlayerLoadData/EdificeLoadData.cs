namespace Vurbiri.Colonization
{
    public readonly struct EdificeLoadData
	{
		public readonly Key key;
		public readonly int id;
		public readonly bool isWall;

		public EdificeLoadData(int[] data)
		{
            key = new(data[0], data[1]);
			id = data[2];
            isWall = data[3] > 0;
        }
    }
}
