using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    public abstract class AStorageOneFile : IStorageService
    {
        protected Dictionary<string, string> _saved = null;
        protected CoroutinesQueue _cnQueue;
        protected string _key;
        protected bool _modified = false;

        public abstract bool IsValid { get; }

        public AStorageOneFile(string key, MonoBehaviour monoBehaviour)
        {
            _key = key;
            _cnQueue = new(monoBehaviour);
        }
        
        public IEnumerator Load_Cn(Action<bool> callback)
        {
            var waitResult = LoadFromFile_Wait();
            yield return waitResult;

            string json = waitResult.Value;
            if (!string.IsNullOrEmpty(json))
            {
                if (TryDeserialize(json, out _saved))
                {
                    callback?.Invoke(true);
                    yield break;
                }
            }

            _saved = new();
            callback?.Invoke(false);
        }

        #region Get(..) / TryGet(..)
        public T Get<T>(string key)
        {
            if (_saved.TryGetValue(key, out string json))
                if (TryDeserialize<T>(json, out T value))
                    return value;

            return default;
        }
        public T Get<T>(string key, JsonConverter converter)
        {
            if (_saved.TryGetValue(key, out string json))
                if (TryDeserialize<T>(json, converter, out T value))
                    return value;

            return default;
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            if (_saved.TryGetValue(key, out string json))
                return TryDeserialize<T>(json, out value);
            return false;
        }
        public bool TryGet<T>(string key, JsonConverter converter, out T value)
        {
            value = default;
            if (_saved.TryGetValue(key, out string json))
                return TryDeserialize<T>(json, converter, out value);
            
            return false;
        }
        #endregion

        #region TryPopulate(..)
        public bool TryPopulate(string key, object obj, JsonConverter converter)
        {
            if (_saved.TryGetValue(key, out string json))
                return Populate(json, obj, converter);
            
            return false;
        }
        public bool TryPopulate<T>(string key, JsonConverter converter)
        {
            if (_saved.TryGetValue(key, out string json))
                return Populate<T>(json, converter);

            return false;
        }
        #endregion

        #region Set(..)
        public bool Set<T>(string key, T data, JsonSerializerSettings settings)
        {
            try
            {
                string json = Serialize<T>(data, settings);
                if (!_saved.TryGetValue(key, out string saveJson) || saveJson != json)
                {
                    _saved[key] = json;
                    _modified = true;
                }
                return true;
            }
            catch (Exception ex) { Log.Info(ex.Message); }

            return false;
        }
        public bool Set<T>(string key, T data, JsonConverter converter)
        {
            JsonSerializerSettings settings = new() { Converters = new List<JsonConverter>(1) { converter } };
            return Set(key, data, settings);
        }
        #endregion

        #region Save(..)
        public void Save(Action<bool> callback)
        {
            _cnQueue.Enqueue(SaveToFile_Cn(callback));
        }
        public void Save<T>(string key, T data, JsonSerializerSettings settings)
        {
            Set(key, data, settings);
            if (_cnQueue.Count == 0)
                _cnQueue.Enqueue(SaveToFile_Cn());
        }
        public void Save<T>(string key, T data, JsonConverter converter)
        {
            Set(key, data, converter);
            if (_cnQueue.Count == 0)
                _cnQueue.Enqueue(SaveToFile_Cn());
        }
        #endregion

        public bool ContainsKey(string key) => _saved.ContainsKey(key);

        public void Remove(string key, bool fromFile)
        {
            _modified |= _saved.Remove(key);

            if (fromFile & _cnQueue.Count == 0)
                _cnQueue.Enqueue(SaveToFile_Cn());
        }

        #region Clear
        public void Clear()
        {
            _modified |= _saved.Count > 0;
            _saved.Clear();

            if (_cnQueue.Count == 0)
                _cnQueue.Enqueue(SaveToFile_Cn());
        }
        public void Clear(string excludeKey)
        {
            if(_saved.Count > 0)
            {
                _saved.Remove(excludeKey, out string restore);
                _saved.Clear();

                if (restore != null) 
                    _saved.Add(excludeKey, restore);

                _modified = true;
            }

            if (_cnQueue.Count == 0)
                _cnQueue.Enqueue(SaveToFile_Cn());
        }
        public void Clear(params string[] excludeKeys)
        {
            if (_saved.Count > 0)
            {
                int count = excludeKeys.Length;
                string[] restores = new string[count];

                for(int i = 0; i < count; i++)
                    _saved.Remove(excludeKeys[i], out restores[i]);

                _saved.Clear();

                for (int i = 0; i < count; i++)
                    if (restores[i] != null) _saved.Add(excludeKeys[i], restores[i]);

                _modified = true;
            }

            if (_cnQueue.Count == 0)
                _cnQueue.Enqueue(SaveToFile_Cn());
        }
        #endregion

        #region SaveToFile_Cn
        protected IEnumerator SaveToFile_Cn(Action<bool> callback)
        {
            if (_modified)
            {
                _modified = false;
                var waitResult = SaveToFile_Wait();
                yield return waitResult;

                _modified |= !waitResult.Value;
                callback?.Invoke(waitResult.Value);

                yield break;
            }

            callback?.Invoke(true);
        }
        protected IEnumerator SaveToFile_Cn()
        {
            if (_modified)
            {
                _modified = false;
                var waitResult = SaveToFile_Wait();
                yield return waitResult;

                _modified |= !waitResult.Value;
            }
        }
        #endregion

        protected abstract WaitResult<string> LoadFromFile_Wait();
        protected abstract WaitResult<bool> SaveToFile_Wait();

        #region Serialize / Deserialize / Populate
        protected string Serialize<T>(T obj, JsonSerializerSettings settings = null) => JsonConvert.SerializeObject(obj, typeof(T), settings);

        protected bool TryDeserialize<T>(string json, out T result)
        {
            result = default;
            if (!string.IsNullOrEmpty(json))
            {
                try { result = JsonConvert.DeserializeObject<T>(json); }
                catch (Exception ex) { Log.Info(ex.Message); }
            }
            return result != null;
        }
        protected bool TryDeserialize<T>(string json, JsonConverter converter, out T result)
        {
            result = default;
            if (!string.IsNullOrEmpty(json))
            {
                try { result = JsonConvert.DeserializeObject<T>(json, converter); }
                catch (Exception ex) { Log.Info(ex.Message); }
            }

            return result != null;
        }

        protected bool Populate<T>(string json, JsonConverter converter)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    JsonConvert.DeserializeObject<T>(json, converter);
                    return true;
                }
                catch (Exception ex) { Log.Info(ex.Message); }
            }

            return false;
        }
        protected bool Populate(string json, object target, JsonConverter converter)
        {
            if (!string.IsNullOrEmpty(json) & target != null)
            {
                JsonSerializerSettings settings = null;
                if (converter != null)
                    settings = new() { Converters = new List<JsonConverter>(1) { converter } };

                try
                {
                    JsonConvert.PopulateObject(json, target, settings);
                    return true;
                }
                catch (Exception ex) { Log.Info(ex.Message); }
            }
            return false;
        }
        #endregion
    }
}
