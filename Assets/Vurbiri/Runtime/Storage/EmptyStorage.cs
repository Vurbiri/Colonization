//Assets\Vurbiri\Runtime\Storage\EmptyStorage.cs
using System;
using System.Collections;

namespace Vurbiri
{
    public class EmptyStorage : IStorageService
    {
        public bool IsValid => false;

        public bool Init(IReadOnlyDIContainer container) => false;

        public IEnumerator Load_Cn(string key, Action<bool> callback)
        {
            callback?.Invoke(false);
            return null;
        }
        public void Save<T>(string key, T data, bool toFile, Action<bool> callback) => callback?.Invoke(false);
        public void Save<T>(string key, T data, float time, Action<bool> callback = null) => callback?.Invoke(false);

        public void Remove(string key, bool toFile, Action<bool> callback) => callback?.Invoke(false);

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            return false;
        }

        public T Get<T>(string key) where T : class => null;

        public bool ContainsKey(string key) => false;

        public void Clear(bool fromFile = true, Action<bool> callback = null) => callback?.Invoke(false);

        public void Clear(string keyExclude, bool fromFile = true, Action<bool> callback = null) => callback?.Invoke(false);

        public void Clear(string[] keyExcludes, bool fromFile = true, Action<bool> callback = null) => callback?.Invoke(false);
    }
}
