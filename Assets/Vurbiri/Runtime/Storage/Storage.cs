//Assets\Vurbiri\Runtime\Storage\Storage.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace Vurbiri
{
    public static class Storage
    {
        public static IEnumerator Create_Cn(DIContainer container, string key, Action<IStorageService> callback)
        {
            if (Create(container, out IStorageService storage))
            {
                bool result = false;
                yield return storage.Load_Cn(key, (b) => result = b);
                Message.Log(result ? "Сохранения загружены" : "Сохранения не найдены");
            }
            else
            {
                Message.Log("StorageService не определён");
            }

            callback?.Invoke(storage);
            container.ReplaceInstance(storage);

            #region Local: Create(...), Creator()
            // =====================
            static bool Create(IReadOnlyDIContainer container, out IStorageService storage)
            {
                IEnumerator<IStorageService> creator = Creator(container);
                while (creator.MoveNext())
                {
                    storage = creator.Current;
                    if (storage.IsValid)
                        return true;
                }

                storage = new EmptyStorage();
                return storage.IsValid;
            }
            // =====================
            static IEnumerator<IStorageService> Creator(IReadOnlyDIContainer container)
            {
                MonoBehaviour monoBehaviour = container.Get<Coroutines>();

                yield return new JsonToYandex(monoBehaviour, container.Get<YandexSDK>());
                yield return new JsonToLocalStorage(monoBehaviour);
                yield return new JsonToCookies(monoBehaviour);
            }
            #endregion
        }

        public static IEnumerator TryLoadTextureWeb_Cn(string url, Action<Return<Texture>> callback)
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

            Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            callback?.Invoke(new(texture != null, texture));
        }

        public static bool LoadObjectFromResourceJson<T>(string path, out T obj)
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
                Message.Log($"--- Не удалось загрузить объект {typeof(T).Name} по пути {path} ---\n".Concat(ex.Message));
                obj = default;
                return false;
            }
        }
    }
}
