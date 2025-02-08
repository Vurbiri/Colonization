//Assets\Vurbiri\Runtime\Utilities\DI\Interface\IReadOnlyDIContainer.cs
namespace Vurbiri
{
    public interface IReadOnlyDIContainer
    {
        public T Get<T>(int id = 0);
        public T Get<T>(TypeIdKey key);

        public bool TryGet<T>(out T instance, int id = 0);
        public bool TryGet<T>(out T instance, TypeIdKey key);

        public T Get<P, T>(P value, int id = 0);
        public T Get<P, T>(P value, TypeIdKey key);

        public bool TryGet<P, T>(out T instance, P value, int id = 0);
        public bool TryGet<P, T>(out T instance, P value, TypeIdKey key);
    }
}
