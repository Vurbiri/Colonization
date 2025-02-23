//Assets\Colonization\Scripts\Data\Interface\IArrayable.cs
namespace Vurbiri.Colonization
{
    public interface IArrayable
	{
        public int[] ToArray();
        public int[] ToArray(int[] array);
    }

    public interface IJaggedArrayable
    {
        public int[][] ToArray();
        public int[][] ToArray(int[][] array);
    }

    public interface IJMoreArrayable
    {
        public int[][][] ToArray();
        public int[][][] ToArray(int[][][] array);
    }
}
