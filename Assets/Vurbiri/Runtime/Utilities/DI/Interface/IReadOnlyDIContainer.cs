namespace Vurbiri
{
    public interface IReadOnlyDIContainer
    {
        public T Get<T>(int id = 0) where T : class;

        public T Get<T>(TypeIdKey key) where T : class;
    }
}
