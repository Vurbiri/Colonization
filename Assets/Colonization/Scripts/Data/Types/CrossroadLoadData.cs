namespace Vurbiri.Colonization
{
    public class CrossroadLoadData
    {
        public Key key = new();
        public int id;
        public bool isWall;

        public void SetValues(int[] arr)
        {
            key.SetValues(arr[0], arr[1]);
            id = arr[2];
            isWall = arr[3] > 0;
        }
    }
}
