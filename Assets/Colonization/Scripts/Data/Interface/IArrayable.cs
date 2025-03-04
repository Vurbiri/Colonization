//Assets\Colonization\Scripts\Data\Interface\IArrayable.cs
namespace Vurbiri.Colonization
{
    public interface IArrayable
	{
        public int[] ToArray();
        public int[] ToArray(int[] array);
    }

    public interface IArrayable<T>
    {
        public T ToArray();
        public T ToArray(T array);
    }

}
