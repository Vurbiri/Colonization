namespace Vurbiri
{
    public interface IValueId<T> where T : AIdType<T>
    {
        public Id<T> Id { get; }
    }
}
