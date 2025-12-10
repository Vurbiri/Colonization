using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public abstract class AStorageOneFile : IStorageService
    {
        private readonly CoroutinesQueue _saveQueue;
        private readonly Stack<WaitResultSource<bool>> _saveWaits = new();
        private bool _modified = false;

        protected readonly string _key;
        protected Dictionary<string, string> _saved = null;

        private WaitResultSource<bool> Wait { [Impl(256)] get => _saveWaits.Count > 0 ? _saveWaits.Pop().Restart() : new(); }

        public bool IsSaved { [Impl(256)] get => !_modified; }
        public abstract bool IsValid { get; }

        [Impl(256)] public AStorageOneFile(string key, MonoBehaviour monoBehaviour)
        {
            _key = key;
            _saveQueue = new(monoBehaviour);
            _saveWaits.Push(new());
        }
        
        public IEnumerator Load_Cn(Out<bool> output)
        {
            var outputJson = LoadFromFile_Wait();
            yield return outputJson;

            if (!TryDeserialize(outputJson, out _saved))
                _saved = new();

            output?.Set(_saved.Count > 0);
        }

        #region Get(..) / TryGet(..)
        [Impl(256)] public T Get<T>(string key)
        {
            if (_saved.TryGetValue(key, out string json) && TryDeserialize<T>(json, out T value))
                return value;
            return default;
        }
        [Impl(256)] public T Get<T>(string key, JsonConverter converter)
        {
            if (_saved.TryGetValue(key, out string json) && TryDeserialize<T>(json, converter, out T value))
                return value;
            return default;
        }

        [Impl(256)] public bool TryGet<T>(string key, out T value)
        {
            value = default;
            return _saved.TryGetValue(key, out string json) && TryDeserialize<T>(json, out value);
        }
        [Impl(256)] public bool TryGet<T>(string key, JsonConverter converter, out T value)
        {
            value = default;
            return _saved.TryGetValue(key, out string json) && TryDeserialize<T>(json, converter, out value);
        }
        #endregion

        #region TryPopulate(..)
        [Impl(256)] public bool TryPopulate(string key, object obj, JsonConverter converter)
        {
            return _saved.TryGetValue(key, out string json) && Populate(json, obj, converter);
        }
        [Impl(256)] public bool TryPopulate<T>(string key, JsonConverter converter)
        {
            return _saved.TryGetValue(key, out string json) && Populate<T>(json, converter);
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
        [Impl(256)] public void Save() => _saveQueue.Enqueue(Save_Cn(Wait));
        public IEnumerator Save(out WaitResult<bool> wait)
        {
            var waitResult = Wait;
            _saveQueue.Enqueue(Save_Cn(waitResult));
            return wait = waitResult;
        }
        public void Save<T>(string key, T data, JsonSerializerSettings settings)
        {
            Set(key, data, settings);
            if (_saveQueue.Count == 0)
                _saveQueue.Enqueue(Save_Cn(Wait));
        }
        public void Save<T>(string key, T data, JsonConverter converter)
        {
            Set(key, data, converter);
            if (_saveQueue.Count == 0)
                _saveQueue.Enqueue(Save_Cn(Wait));
        }
        #endregion

        [Impl(256)] public bool ContainsKey(string key) => _saved.ContainsKey(key);

        public void Remove(string key, bool fromFile)
        {
            _modified |= _saved.Remove(key);

            if (fromFile & _saveQueue.Count == 0)
                _saveQueue.Enqueue(Save_Cn(Wait));
        }

        #region Clear
        public void Clear()
        {
            _modified |= _saved.Count > 0;
            _saved.Clear();

            if (_saveQueue.Count == 0)
                _saveQueue.Enqueue(Save_Cn(Wait));
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

            if (_saveQueue.Count == 0)
                _saveQueue.Enqueue(Save_Cn(Wait));
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

            if (_saveQueue.Count == 0)
                _saveQueue.Enqueue(Save_Cn(Wait));
        }
        #endregion

        #region SaveToFile_Cn
        protected IEnumerator Save_Cn(WaitResultSource<bool> result)
        {
            if (_modified)
            {
                _modified = false;
                yield return SaveToFile_Cn(result);
                _modified |= !result;
            }
            else
            {
                result.Set(true);
            }

            yield return null;

            _saveWaits.Push(result);
        }
        #endregion

        protected abstract WaitResult<string> LoadFromFile_Wait();
        protected abstract IEnumerator SaveToFile_Cn(WaitResultSource<bool> waitResult);

        #region Serialize / Deserialize / Populate
        [Impl(256)] protected string Serialize<T>(T obj, JsonSerializerSettings settings = null) => JsonConvert.SerializeObject(obj, typeof(T), settings);

        protected bool TryDeserialize<T>(string json, out T output)
        {
            output = default;
            if (!string.IsNullOrEmpty(json))
            {
                try { output = JsonConvert.DeserializeObject<T>(json); return true; }
                catch (Exception ex) { Log.Info(ex.Message); }
            }
            return false;
        }
        protected bool TryDeserialize<T>(string json, JsonConverter converter, out T output)
        {
            output = default;
            if (!string.IsNullOrEmpty(json))
            {
                try { output = JsonConvert.DeserializeObject<T>(json, converter); return true; }
                catch (Exception ex) { Log.Info(ex.Message); }
            }
            return false;
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
