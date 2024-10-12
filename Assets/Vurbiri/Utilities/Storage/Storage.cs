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

        public static bool StoragesCreate(IReadOnlyDIContainer container)
        {
            if (Create<JsonToYandex>(container))
                return true;

            if (Create<JsonToLocalStorage>(container))
                return true;

            if (Create<JsonToCookies>(container))
                return true;

            Create<EmptyStorage>(container);
            return false;

            #region Local: Create<T>()
            // =====================
            static bool Create<T>(IReadOnlyDIContainer container) where T : ASaveLoadJsonTo, new()
            {
                if (service != null && typeof(T) == service.GetType())
                    return true;

                service = new T();
                return service.Init(container);
            }
            #endregion
        }
        public static IEnumerator Load_Coroutine(string key, Action<bool> callback) => service.Load_Coroutine(key, callback);
        public static IEnumerator Save_Coroutine(string key, object data, bool toFile = true, Action<bool> callback = null) => service.Save_Coroutine(key, data, toFile, callback);
        public static Return<T> Get<T>(string key) where T : class => service.Get<T>(key);
        public static bool TryGet<T>(string key, out T value) where T : class => service.TryGet<T>(key, out value);
        public static bool ContainsKey(string key) => service.ContainsKey(key);
                
        public static IEnumerator TryLoadTextureWeb_Coroutine(string url, Action<Return<Texture>> callback)
        {
            if (string.IsNullOrEmpty(url) || !url.StartsWith("https://"))
            {
                callback?.Invoke(Return<Texture>.Empty);
                yield break;
            }

            using var request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result != Result.Success || request.downloadHandler == null)
            {
                Message.Log("==== UnityWebRequest: " + request.error);
                callback?.Invoke(Return<Texture>.Empty);
                yield break;
            }

            callback?.Invoke(new(((DownloadHandlerTexture)request.downloadHandler).texture));
        }

        public static bool LoadObjectFromResourceJson<T>(string path, out T obj) where T : class
        {
            try
            {
                var textAsset = Resources.Load<TextAsset>(path);
                obj = JsonConvert.DeserializeObject<T>(textAsset.text);
                Resources.UnloadAsset(textAsset);
                return true;
            }
            catch (Exception ex)
            {
                Message.Error($"--- Ошибка загрузки объекта {typeof(T).Name} по пути {path} ---\n".Concat(ex.Message));
                obj = null;
                return false;
            }
        }
    }
}
