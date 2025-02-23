//Assets\Vurbiri\Runtime\Storage\EmptyStorage.cs
using System;
using System.Collections;

namespace Vurbiri
{
    public class EmptyStorage : IStorageService
    {
        public bool IsValid => false;

        public bool Init(IReadOnlyDIContainer container) => false;

        public IEnumerator Load_Coroutine(string key, Action<bool> callback)
        {
            callback?.Invoke(false);
            return null;
        }
        public IEnumerator Save_Coroutine<T>(string key, T data, bool toFile = true, Action<bool> callback = null)
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

        
    }
}
