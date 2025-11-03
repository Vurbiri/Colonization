namespace Vurbiri
{
    public abstract class Log
    {
        private static Log s_instance;

        public static void Create<T>() where T : Log, new() => s_instance = new T();

#if UNITY_EDITOR
        public static void Info(string msg) => UnityEngine.Debug.Log(msg);
        public static void Warning(string msg) => UnityEngine.Debug.LogWarning(msg);
        public static void Error(string msg) => UnityEngine.Debug.LogError(msg);
#else
        public static void Info(string msg) => s_instance.InfoRuntime(msg);
        public static void Warning(string msg) => s_instance.WarningRuntime(msg);
        public static void Error(string msg) => s_instance.ErrorRuntime(msg);
#endif

        public static void Info(object obj) => Info(obj.ToString());
        public static void Warning(object obj) => Warning(obj.ToString());
        public static void Error(object obj) => Error(obj.ToString());

        protected abstract void InfoRuntime(string msg);
        protected abstract void WarningRuntime(string msg);
        protected abstract void ErrorRuntime(string msg);
    }
}

