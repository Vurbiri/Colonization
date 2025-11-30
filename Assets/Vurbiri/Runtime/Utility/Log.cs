using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public abstract class Log
    {
        private static Log s_instance;

        [Impl(256)] public static void Create<T>() where T : Log, new() => s_instance = new T();

#if UNITY_EDITOR
        public static void Info(string msg) => UnityEngine.Debug.Log(msg);
        public static void Warning(string msg) => UnityEngine.Debug.LogWarning(msg);
        public static void Error(string msg) => UnityEngine.Debug.LogError(msg);
#else
        [Impl(256)] public static void Info(string msg) => s_instance.InfoInternal(msg);
        [Impl(256)] public static void Warning(string msg) => s_instance.WarningInternal(msg);
        [Impl(256)] public static void Error(string msg) => s_instance.ErrorInternal(msg);
#endif

        [Impl(256)] public static void Info(object obj) => Info(obj.ToString());
        [Impl(256)] public static void Warning(object obj) => Warning(obj.ToString());
        [Impl(256)] public static void Error(object obj) => Error(obj.ToString());

        protected abstract void InfoInternal(string msg);
        protected abstract void WarningInternal(string msg);
        protected abstract void ErrorInternal(string msg);
    }
}

