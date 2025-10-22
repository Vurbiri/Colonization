using Newtonsoft.Json;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace Vurbiri
{
    public static class Storage
    {
        public static IEnumerator LoadTextureFromWeb_Cn(string url, Out<Return<Texture>> output)
        {
            Texture texture = null;
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http"))
            {
                using var request = UnityWebRequestTexture.GetTexture(url);
                yield return request.SendWebRequest();

                if (request.result != Result.Success | request.downloadHandler == null)
                    Log.Warning($"[Error::UnityWebRequest] {request.error}");
                else
                    texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }

            output.Set(new(texture != null, texture));
        }

        public static bool TryLoadObjectFromJsonResource<T>(string path, out T obj, bool logWarning = true)
        {
            try
            {
                obj = (T)LoadObjectFromJsonResource(path, typeof(T));
                return true;
            }
            catch (Exception ex)
            {
                if(logWarning) Log.Warning($"[Error::Storage] Failed to load object {typeof(T).Name} on the path {path}.\n".Concat(ex.Message));
                obj = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T LoadObjectFromJsonResource<T>(string path) => (T)LoadObjectFromJsonResource(path, typeof(T));
        public static object LoadObjectFromJsonResource(string path, Type type)
        {
            var textAsset = Resources.Load<TextAsset>(path);
            var obj = JsonConvert.DeserializeObject(textAsset.text, type, settings: null);
            Resources.UnloadAsset(textAsset);
            return obj;
        }
    }
}
