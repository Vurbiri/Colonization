using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace Vurbiri
{
    public class Storage : IStorageService
    {
        private IStorageService _service;

        public bool IsValid => _service != null && _service.IsValid; 

        public bool Init(IReadOnlyDIContainer container)
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
            bool Create<T>(IReadOnlyDIContainer container) where T : IStorageService, new()
            {
                if (_service != null && typeof(T) == _service.GetType())
                    return true;

                _service = new T();
                return _service.Init(container);
            }
            #endregion
        }
        public IEnumerator Load_Coroutine(string key, Action<bool> callback) => _service.Load_Coroutine(key, callback);
        public IEnumerator Save_Coroutine(string key, object data, bool toFile = true, Action<bool> callback = null) => _service.Save_Coroutine(key, data, toFile, callback);
        public Return<T> Get<T>(string key) where T : class => _service.Get<T>(key);
        public bool TryGet<T>(string key, out T value) where T : class => _service.TryGet<T>(key, out value);
                
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
                Message.Error($"--- ������ �������� ������� {typeof(T).Name} �� ���� {path} ---\n".Concat(ex.Message));
                obj = null;
                return false;
            }
        }
    }
}
