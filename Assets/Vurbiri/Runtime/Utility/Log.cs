using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public abstract class Log
    {
#if UNITY_EDITOR
        public static void Create<T>() where T : Log, new() { T test = new(); }

        public static void Info(string msg) => UnityEngine.Debug.Log(msg);
        public static void Warning(string msg) => UnityEngine.Debug.LogWarning(msg);
        public static void Error(string msg) => UnityEngine.Debug.LogError(msg);
#else
        private static Log s_instance;

        [Impl(256)] public static void Create<T>() where T : Log, new() => s_instance = new T();

        [Impl(256)] public static void Info(string msg) => s_instance.InfoInternal(msg);
        [Impl(256)] public static void Warning(string msg) => s_instance.WarningInternal(msg);
        [Impl(256)] public static void Error(string msg) => s_instance.ErrorInternal(msg);
#endif

        [Impl(256)] public static void Info(object obj) => Info(ToString(obj));
        [Impl(256)] public static void Warning(object obj) => Warning(ToString(obj));
        [Impl(256)] public static void Error(object obj) => Error(ToString(obj));

        protected abstract void InfoInternal(string msg);
        protected abstract void WarningInternal(string msg);
        protected abstract void ErrorInternal(string msg);

        [Impl(256)] private static string ToString(object obj) => obj is null ? "null" : obj.ToString();
    }
}

