//Assets\Vurbiri\Runtime\Utilities\DI\Interface\IReadOnlyDIContainer.cs
namespace Vurbiri
{
    public interface IReadOnlyDIContainer
    {
        public T Get<T>(int id = 0);
        public T Get<T>(TypeIdKey key);

        public T Get<P, T>(P value, int id = 0);
        public T Get<P, T>(P value, TypeIdKey key);
    }
}
