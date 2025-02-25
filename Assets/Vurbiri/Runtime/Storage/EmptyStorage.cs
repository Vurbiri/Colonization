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
        public IEnumerator Save_Cn<T>(string key, T data, bool toFile, Action<bool> callback)
        {
            callback?.Invoke(false);
            return null;
        }

        public IEnumerator Remove_Cn(string key, bool toFile, Action<bool> callback)
        {
            callback?.Invoke(false);
            return null;
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            return false;
        }

        public T Get<T>(string key) where T : class => null;

        public bool ContainsKey(string key) => false;

        public IEnumerator Clear_Cn(bool fromFile = true, Action<bool> callback = null)
        {
            callback?.Invoke(false);
            return null;
        }

        public IEnumerator Clear_Cn(string keyExclude, bool fromFile = true, Action<bool> callback = null)
        {
            callback?.Invoke(false);
            return null;
        }

        public IEnumerator Clear_Cn(string[] keyExcludes, bool fromFile = true, Action<bool> callback = null)
        {
            callback?.Invoke(false);
            return null;
        }
    }
}
