namespace Vurbiri.EntryPoint
{
    public abstract class AEnterParam
    {
        public T Get<T>() where T : AEnterParam => (T)this;
    }
}
