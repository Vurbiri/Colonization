using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
    public abstract class ASaveLoadJsonTo
    {
        protected Dictionary<string, string> _saved = null;
        protected string _key;
        protected bool _dictModified = false;

        public abstract bool IsValid { get; }

        public abstract IEnumerator Initialize_Coroutine(string key, Action<bool> callback);

        public virtual Return<T> Load<T>(string key) where T : class
        {
            if (_saved.TryGetValue(key, out string json))
                return Deserialize<T>(json);

            return Return<T>.Empty;
        }
        public IEnumerator Save_Coroutine(string key, object data, bool toFile, Action<bool> callback)
        {
            bool result = SaveToMemory();
            if (!toFile || !(result && _dictModified))
            {
                callback?.Invoke(result);
                yield break;
            }

            yield return SaveToFile_Coroutine();

            #region Local: SaveToMemory(), SaveToFile_Coroutine()
            //======================================
            bool SaveToMemory()
            {
                try
                {
                    string json = Serialize(data);
                    _saved.TryGetValue(key, out string saveJson);
                    if (saveJson == null || saveJson != json)
                    {
                        _saved[key] = json;
                        _dictModified = true;
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

                result = waitResult.Result;
                _dictModified = !result;
                callback?.Invoke(result);
            }
            #endregion
        }

        public bool ContainsKey(string key) => _saved.ContainsKey(key);

        protected abstract WaitResult<bool> SaveToFile_Wait();

        protected string Serialize(object obj) => Storage.Serialize(obj);
        protected Return<T> Deserialize<T>(string json) where T : class => Storage.Deserialize<T>(json);
    }
}
