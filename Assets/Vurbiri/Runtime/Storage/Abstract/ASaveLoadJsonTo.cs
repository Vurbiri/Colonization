//Assets\Vurbiri\Runtime\Storage\Abstract\ASaveLoadJsonTo.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    public abstract class ASaveLoadJsonTo : IStorageService
    {
        protected Dictionary<string, string> _saved = null;
        protected string _key;
        protected bool _modified = false;

        public abstract bool IsValid { get; }

        public abstract bool Init(IReadOnlyDIContainer container);

        public abstract IEnumerator Load_Coroutine(string key, Action<bool> callback);

        public virtual Return<T> Get<T>(string key)
        {
            if (_saved.TryGetValue(key, out string json))
                return Deserialize<T>(json);

            return Return<T>.Empty;
        }

        public virtual bool TryGet<T>(string key, out T value)
        {
            Return<T> result = Get<T>(key);
            value = result.Value;
            return result.Result;
        }

        public IEnumerator Save_Coroutine<T>(string key, T data, bool toFile, Action<bool> callback)
        {
            bool result = SaveToMemory(data);
            if (!toFile | !(result & _modified))
            {
                callback?.Invoke(result);
                yield break;
            }

            yield return SaveToFile_Coroutine();

            #region Local: SaveToMemory(..), SaveToFile_Coroutine()
            //======================================
            bool SaveToMemory(T data)
            {
                try
                {
                    string json = Serialize<T>(data);

                    if (!_saved.TryGetValue(key, out string saveJson) || saveJson != json)
                    {
                        _saved[key] = json;
                        _modified = true;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Message.Log(ex.Message);
                }

                return false;
            }
            //======================================
            IEnumerator SaveToFile_Coroutine()
            {
                WaitResult<bool> waitResult = SaveToFile_Wait();
                yield return waitResult;

                _modified = !waitResult.Result;
                callback?.Invoke(waitResult.Result);
            }
            #endregion
        }

        public bool ContainsKey(string key) => _saved.ContainsKey(key);

        protected abstract WaitResult<bool> SaveToFile_Wait();

        protected virtual string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, typeof(T), null);

        protected virtual Return<T> Deserialize<T>(string json)
        {
            Return<T> result = Return<T>.Empty;
            try
            {
                if(!string.IsNullOrEmpty(json))
                    result = new(JsonConvert.DeserializeObject<T>(json));
            }
            catch (Exception ex)
            {
                Message.Log(ex.Message);
            }

            return result;
        }
    }
}
