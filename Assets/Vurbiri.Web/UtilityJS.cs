using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace Vurbiri.Web
{
    public static partial class UtilityJS
    {
        public static IEnumerator LoadTexture_Cn(string url, Out<Return<Texture>> output)
        {
            Texture texture = null;
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http"))
            {
                using var request = UnityWebRequestTexture.GetTexture(url);
                yield return request.SendWebRequest();

                if (request.result != Result.Success | request.downloadHandler == null)
                    Vurbiri.Log.Warning($"[Error::UnityWebRequest] {request.error}");
                else
                    texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }

            output.Set(new(texture != null, texture));
        }

#if !UNITY_EDITOR
    public static bool IsMobile => IsMobileUnityJS();

    public static void Log(string message) => LogJS(message);
    public static void Error(string message) => ErrorJS(message);

    public static bool SetStorage(string key, string data) => SetStorageJS(key, data);
    public static string GetStorage(string key) => GetStorageJS(key);
    public static bool IsStorage() => IsStorageJS();

    public static bool SetCookies(string key, string data) => SetCookiesJS(key, data);
    public static string GetCookies(string key) => GetCookiesJS(key);
    public static bool IsCookies() => IsCookiesJS();
#endif
    }
}
