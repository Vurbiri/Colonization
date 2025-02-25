//Assets\Vurbiri\Runtime\Storage\Interface\IStorageService.cs
using System;
using System.Collections;

namespace Vurbiri
{
    public interface IStorageService
    {
        public bool IsValid { get; }

        public bool Init(IReadOnlyDIContainer container);

        public IEnumerator Load_Coroutine(string key, Action<bool> callback);

        public IEnumerator Save_Coroutine<T>(string key, T data, bool toFile = true, Action<bool> callback = null);

        public IEnumerator Remove_Coroutine(string key, bool fromFile = true, Action<bool> callback = null);

        public bool TryGet<T>(string key, out T value);

        public T Get<T>(string key) where T : class;

        public bool ContainsKey(string key);
    }
}
