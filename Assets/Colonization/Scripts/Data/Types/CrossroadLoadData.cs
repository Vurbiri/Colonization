namespace Vurbiri.Colonization
{
    public class CrossroadLoadData
    {
        public Key key = new();
        public EdificeType type;
        public bool isWall;

        public void SetValues(int[] arr)
        {
            key.SetValues(arr[0], arr[1]);
            type = (EdificeType)arr[2];
            isWall = arr[3] > 0;
        }
    }
}
