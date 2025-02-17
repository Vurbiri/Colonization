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

        public IEnumerator Save_Coroutine(string key, object data, bool toFile = true, Action<bool> callback = null);

        public Return<T> Get<T>(string key);

        public bool TryGet<T>(string key, out T value);
    }
}
