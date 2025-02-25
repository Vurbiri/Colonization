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

        public abstract IEnumerator Load_Cn(string key, Action<bool> callback);

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            if (_saved.TryGetValue(key, out string json))
            {
                Return<T> result = Deserialize<T>(json);
                value = result.Value;
                return result.Result;
            }
            return false;
        }

        public virtual T Get<T>(string key) where T : class
        {
            if (_saved.TryGetValue(key, out string json))
                return Deserialize<T>(json).Value;

            return null;
        }

        public IEnumerator Save_Cn<T>(string key, T data, bool toFile, Action<bool> callback)
        {
            bool result = SaveToMemory(key, data);
            if (!(toFile & _modified))
            {
                callback?.Invoke(result);
                yield break;
            }

            yield return SaveToFile_Cn(callback);
        }

        public IEnumerator Remove_Cn(string key, bool fromFile, Action<bool> callback)
        {
            bool result = _saved.Remove(key);
            _modified |= result;
            if (!(fromFile & _modified))
            {
                callback?.Invoke(result);
                yield break;
            }

            yield return SaveToFile_Cn(callback);
        }

        #region Clear
        public IEnumerator Clear_Cn(bool fromFile, Action<bool> callback)
        {
            _modified |= _saved.Count > 0;
            _saved.Clear();
            if (!(fromFile & _modified))
            {
                callback?.Invoke(true);
                yield break;
            }

            yield return SaveToFile_Cn(callback);
        }
        public IEnumerator Clear_Cn(string keyExclude, bool fromFile, Action<bool> callback)
        {
            if(_saved.Count > 0)
            {
                _saved.Remove(keyExclude, out string restore);

                _saved.Clear();
                _modified = true;

                if (restore != null) 
                    _saved.Add(keyExclude, restore);
            }
            if (!(fromFile & _modified))
            {
                callback?.Invoke(true);
                yield break;
            }

            yield return SaveToFile_Cn(callback);
        }
        public IEnumerator Clear_Cn(string[] keyExcludes, bool fromFile, Action<bool> callback)
        {
            if (_saved.Count > 0)
            {
                int count = keyExcludes.Length;
                string[] values = new string[count];

                for(int i = 0; i < count; i++)
                    _saved.Remove(keyExcludes[i], out values[i]);

                _saved.Clear();
                _modified = true;

                for (int i = 0; i < count; i++)
                {
                    if (values[i] != null)
                        _saved.Add(keyExcludes[i], values[i]);
                }
            }
            if (!(fromFile & _modified))
            {
                callback?.Invoke(true);
                yield break;
            }

            yield return SaveToFile_Cn(callback);
        }
        #endregion

        public bool ContainsKey(string key) => _saved.ContainsKey(key);


        protected bool SaveToMemory<T>(string key, T data)
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

        protected IEnumerator SaveToFile_Cn(Action<bool> callback)
        {
            WaitResult<bool> waitResult = SaveToFile_Wt();
            yield return waitResult;

            _modified = !waitResult.Result;
            callback?.Invoke(waitResult.Result);
        }
        protected abstract WaitResult<bool> SaveToFile_Wt();


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
