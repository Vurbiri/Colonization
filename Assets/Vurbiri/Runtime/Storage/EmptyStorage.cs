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

        public void Save(Action<bool> callback = null) => callback?.Invoke(false);
        public void Save<T>(string key, T data, Action<bool> callback = null) => callback?.Invoke(false);

        public bool Set<T>(string key, T data) => false;

        public T Get<T>(string key) where T : class => null;

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            return false;
        }

        public bool ContainsKey(string key) => false;

        public void Remove(string key, bool toFile, Action<bool> callback) => callback?.Invoke(false);

        public void Clear(Action<bool> callback = null) => callback?.Invoke(false);

        public void Clear(string keyExclude, Action<bool> callback = null) => callback?.Invoke(false);

        public void Clear(string[] keyExcludes, Action<bool> callback = null) => callback?.Invoke(false);
    }
}
