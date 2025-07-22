namespace Vurbiri.EntryPoint
{
	public class AContainer<T> : IContainer where T : IContainerContent
    {
        protected static T s_content;

        public AContainer(T content)
        {
            s_content ??= content;
        }

        public static AContainer<T> Create(T content)
        {
            return new AContainer<T>(content);
        }

        public virtual void Dispose()
        {
            s_content.Dispose();
        }
    }
}
