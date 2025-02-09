//Assets\Vurbiri\Runtime\Types\Collections\IdCollections\Interface\IValueId.cs
namespace Vurbiri
{
    public interface IValueId<T> where T : IdType<T>
    {
        public Id<T> Id { get; }

    }
}
