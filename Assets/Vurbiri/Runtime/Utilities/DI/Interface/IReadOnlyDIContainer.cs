//Assets\Vurbiri\Runtime\Utilities\DI\Interface\IReadOnlyDIContainer.cs
namespace Vurbiri
{
    public interface IReadOnlyDIContainer
    {
        public T Get<T>();
        public bool TryGet<T>(out T instance);

        public T Get<P, T>(P value);
        public bool TryGet<P, T>(out T instance, P value);
    }
}
