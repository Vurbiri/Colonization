using System.Runtime.InteropServices;

namespace Vurbiri.Web
{
    public partial class UtilityJS
    {
        [DllImport("__Internal")]
        internal static extern bool IsMobileUnityJS();
        [DllImport("__Internal")]
        internal static extern void LogJS(string msg);
        [DllImport("__Internal")]
        internal static extern void ErrorJS(string msg);
        [DllImport("__Internal")]
        internal static extern bool SetStorageJS(string key, string data);
        [DllImport("__Internal")]
        internal static extern string GetStorageJS(string key);
        [DllImport("__Internal")]
        internal static extern bool IsStorageJS();
        [DllImport("__Internal")]
        internal static extern bool SetCookiesJS(string key, string data);
        [DllImport("__Internal")]
        internal static extern string GetCookiesJS(string key);
        [DllImport("__Internal")]
        internal static extern bool IsCookiesJS();
    }
}
