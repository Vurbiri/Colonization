
namespace Vurbiri
{
    public static class Log
    {
        public static void Info(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#else
            UtilityJS.Log(msg);
#endif
        }
        public static void Info(object obj) => Info(obj.ToString());

        public static void Warning(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(msg);
#else
            UtilityJS.Log(msg);
#endif
        }
        public static void Warning(object obj) => Warning(obj.ToString());

        public static void Error(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(msg);
#else
            UtilityJS.Error(msg);
#endif
        }
        public static void Error(object obj) => Error(obj.ToString());
    }
}

