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
    public class Storage
    {
        public static IEnumerator Create_Coroutine(DIContainer container, string key)
        {
            if (Create(container, out IStorageService storage))
            {
                bool result = false;
                yield return storage.Load_Coroutine(key, (b) => result = b);
                Message.Log(result ? "Сохранения загружены" : "Сохранения не найдены");
            }
            else
            {
                Message.Log("StorageService не определён");
            }

            container.ReplaceInstance<IStorageService>(storage);

            #region Local: Create(...), Creator()
            // =====================
            static bool Create(IReadOnlyDIContainer container, out IStorageService storage)
            {
                IEnumerator<IStorageService> creator = Creator();
                while (creator.MoveNext())
                {
                    storage = creator.Current;
                    if (storage.Init(container))
                        return true;
                }

                storage = new EmptyStorage();
                return storage.Init(container);
            }
            // =====================
            static IEnumerator<IStorageService> Creator()
            {
                yield return new JsonToYandex();
                yield return new JsonToLocalStorage();
                yield return new JsonToCookies();
            }
            #endregion
        }

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
                Message.Log($"--- Не удалось загрузить объект {typeof(T).Name} по пути {path} ---\n".Concat(ex.Message));
                obj = null;
                return false;
            }
        }
    }
}
