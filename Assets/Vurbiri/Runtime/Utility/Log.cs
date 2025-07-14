namespace Vurbiri
{
    public enum MessageType
    {
        Normal,
        Warning,
        Error,
        FatalError
    }

    public static class Log
    {
        public static void Msg(string msg) => UtilityJS.Log(msg);
        public static void Error(string msg) => UtilityJS.Error(msg);
    }
}

