namespace Vurbiri
{
    public class SceneContainer
	{
        private static DIContainer s_container;

        public DIContainer Container => s_container;

        public SceneContainer(IReadOnlyDIContainer parent)
        {
            Dispose(); s_container = new(parent);
        }

        public static T Get<T>() => s_container.Get<T>();
        public static bool TryGet<T>(out T instance) => s_container.TryGet(out instance);

        public static T Get<P, T>(P value) => s_container.Get<P, T>(value);
        public static bool TryGet<P, T>(out T instance, P value) => s_container.TryGet(out instance, value);

        public void Dispose()
        {
            s_container?.Dispose();
            s_container = null;
        }
    }
}
