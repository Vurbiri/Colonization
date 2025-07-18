using UnityEngine;

namespace Vurbiri
{
    public static class Log
    {
        public static void Info(string msg)
        {
#if UNITY_EDITOR
            Debug.Log(msg);
#else
            UtilityJS.Log(msg);
#endif
        }

        public static void Warning(string msg)
        {
#if UNITY_EDITOR
            Debug.LogWarning(msg);
#else
            UtilityJS.Log(msg);
#endif
        }

        public static void Error(string msg)
        {
#if UNITY_EDITOR
            Debug.LogError(msg);
#else
            UtilityJS.Error(msg);
#endif
        }
    }
}

