using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    public abstract class HintId : IdType<HintId>
    {
        public const int Canvas = 0;
        public const int World = 1;

        [Impl(256)] public static int Get<THint>() => Get(typeof(THint));
        [Impl(256)] public static int Get(System.Type type) => type == typeof(CanvasHint) ? Canvas : World;
    }
}
