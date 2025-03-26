//Assets\Vurbiri\Runtime\Storage\Abstract\ASaveLoadJsonTo.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public abstract class ASaveLoadJsonTo : IStorageService
    {
        protected Dictionary<string, string> _saved = null;
        protected CoroutinesQueue _cnQueue;
        protected string _key;
        protected bool _modified = false;

        public abstract bool IsValid { get; }

        public abstract bool Init(IReadOnlyDIContainer container);
        
        public abstract IEnumerator Load_Cn(string key, Action<bool> callback);

        public virtual T Get<T>(string key) where T : class
        {
            if (_saved.TryGetValue(key, out string json))
                return Deserialize<T>(json).Value;

            return null;
        }

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

        public bool TryGet<T>(string key, JsonConverter converter, out T value)
        {
            value = default;
            if (_saved.TryGetValue(key, out string json))
            {
                Return<T> result = Deserialize<T>(json, converter);
                value = result.Value;
                return result.Result;
            }
            return false;
        }

        public bool TryPopulate(string key, object obj, JsonConverter converter)
        {
            if (_saved.TryGetValue(key, out string json))
                return Populate(json, obj, converter);
            
            return false;
        }

        public bool Set<T>(string key, T data, JsonConverter converter)
        {
            try
            {
                string json = Serialize<T>(data, converter);
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

        public void Save(Action<bool> callback) => _cnQueue.Enqueue(SaveToFile_Cn(callback));
        public void Save<T>(string key, T data, JsonConverter converter, Action<bool> callback)
        {
            bool result = Set(key, data, converter);
            if (_cnQueue.Count > 0)
            {
                callback?.Invoke(result);
                return;
            }

            _cnQueue.Enqueue(SaveToFile_Cn(callback));
        }

        public void Remove(string key, bool fromFile, Action<bool> callback)
        {
            bool result = _saved.Remove(key);
            _modified |= result;

            if (!fromFile | _cnQueue.Count > 0)
            {
                callback?.Invoke(result);
                return;
            }

            _cnQueue.Enqueue(SaveToFile_Cn(callback));
        }

        #region Clear
        public void Clear(Action<bool> callback)
        {
            _modified |= _saved.Count > 0;
            _saved.Clear();
            if (_cnQueue.Count > 0)
            {
                callback?.Invoke(true);
                return;
            }

            _cnQueue.Enqueue(SaveToFile_Cn(callback));
        }
        public void Clear(string excludeKey, Action<bool> callback)
        {
            if(_saved.Count > 0)
            {
                _saved.Remove(excludeKey, out string restore);

                _saved.Clear();
                _modified = true;

                if (restore != null) 
                    _saved.Add(excludeKey, restore);
            }

            if (_cnQueue.Count > 0)
            {
                callback?.Invoke(true);
                return;
            }

            _cnQueue.Enqueue(SaveToFile_Cn(callback));
        }
        public void Clear(string[] excludeKeys, Action<bool> callback)
        {
            if (_saved.Count > 0)
            {
                int count = excludeKeys.Length;
                string[] values = new string[count];

                for(int i = 0; i < count; i++)
                    _saved.Remove(excludeKeys[i], out values[i]);

                _saved.Clear();
                _modified = true;

                for (int i = 0; i < count; i++)
                {
                    if (values[i] != null)
                        _saved.Add(excludeKeys[i], values[i]);
                }
            }

            if (_cnQueue.Count > 0)
            {
                callback?.Invoke(true);
                return;
            }

            _cnQueue.Enqueue(SaveToFile_Cn(callback));
        }
        #endregion

        public bool ContainsKey(string key) => _saved.ContainsKey(key);

        protected void Init(MonoBehaviour monoBehaviour) => _cnQueue = new(monoBehaviour);

        protected IEnumerator SaveToFile_Cn(Action<bool> callback)
        {
            if (_modified)
            {
                WaitResult<bool> waitResult = SaveToFile_Wait();
                yield return waitResult;

                _modified = !waitResult.Value;
                callback?.Invoke(waitResult.Value);

                yield break;
            }

            callback?.Invoke(false);
        }

        protected abstract WaitResult<bool> SaveToFile_Wait();

        protected virtual string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, typeof(T), null);
        protected virtual string Serialize<T>(T obj, JsonConverter converter)
        {
            JsonSerializerSettings settings = null;
            if (converter != null)
                settings = new() { Converters = new List<JsonConverter>(1) { converter } };

            return JsonConvert.SerializeObject(obj, typeof(T), settings);
        }

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

        protected virtual Return<T> Deserialize<T>(string json, JsonConverter converter)
        {
            Return<T> result = Return<T>.Empty;
            try
            {
                if (!string.IsNullOrEmpty(json))
                    result = new(JsonConvert.DeserializeObject<T>(json, converter));
            }
            catch (Exception ex)
            {
                Message.Log(ex.Message);
            }

            return result;
        }

        protected virtual bool Populate(string json, object target, JsonConverter converter)
        {
            JsonSerializerSettings settings = null;
            if (converter != null)
                settings = new() { Converters = new List<JsonConverter>(1) { converter } };

            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    JsonConvert.PopulateObject(json, target, settings);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Message.Log(ex.Message);
            }

            return false;
        }
    }
}
