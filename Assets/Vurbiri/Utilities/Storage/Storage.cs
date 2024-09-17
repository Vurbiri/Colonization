using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace Vurbiri
{
    public static class Storage
    {
        private static ASaveLoadJsonTo service;

        public static Type TypeStorage => service?.GetType();

        public static bool StoragesCreate()
        {
            if (Create<JsonToYandex>())
                return true;

            if (Create<JsonToLocalStorage>())
                return true;

            if (Create<JsonToCookies>())
                return true;

            Create<EmptyStorage>();
            return false;

            #region Local Function
            static bool Create<T>() where T : ASaveLoadJsonTo, new()
            {
                if (typeof(T) == TypeStorage)
                    return true;

                service = new T();
                return service.IsValid;
            }
            #endregion
        }
        public static IEnumerator Initialize_Coroutine(string key, Action<bool> callback) => service.Initialize_Coroutine(key, callback);
        public static IEnumerator Save_Coroutine(string key, object data, bool toFile = true, Action<bool> callback = null) => service.Save_Coroutine(key, data, toFile, callback);
        public static Return<T> Load<T>(string key) where T : class => service.Load<T>(key);
        public static bool ContainsKey(string key) => service.ContainsKey(key);
                
        public static IEnumerator TryLoadTextureWeb(string url, Action<Return<Texture>> callback)
        {
            if (string.IsNullOrEmpty(url) || !url.StartsWith("https://"))
            {
                callback?.Invoke(Return<Texture>.Empty);
                yield break;
            }

            using (var request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.result != Result.Success || request.downloadHandler == null)
                {
                    Message.Log("==== UnityWebRequest: " + request.error);
                    callback?.Invoke(Return<Texture>.Empty);
                    yield break;
                }

                callback?.Invoke(new(((DownloadHandlerTexture)request.downloadHandler).texture));
            }
        }

        public static bool LoadResourceFromJson<T>(string path, out T resource) where T : class
        {
            try
            {
                var textAsset = Resources.Load<TextAsset>(path);
                resource = JsonConvert.DeserializeObject<T>(textAsset.text);
                Resources.UnloadAsset(textAsset);
                return true;
            }
            catch (Exception ex)
            {
                Message.Error($"--- Ошибка загрузки: {path} ---\n".Concat(ex.Message));
                resource = null;
                return false;
            }
        }
    }
}
