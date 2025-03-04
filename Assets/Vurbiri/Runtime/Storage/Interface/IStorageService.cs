//Assets\Vurbiri\Runtime\Storage\Interface\IStorageService.cs
using System;
using System.Collections;

namespace Vurbiri
{
    public interface IStorageService
    {
        public bool IsValid { get; }

        public bool Init(IReadOnlyDIContainer container);

        public IEnumerator Load_Cn(string key, Action<bool> callback);

        public void Save<T>(string key, T data, bool toFile = true, Action<bool> callback = null);
        public void Save<T>(string key, T data, float time, Action<bool> callback = null);

        public bool TryGet<T>(string key, out T value);

        public T Get<T>(string key) where T : class;

        public bool ContainsKey(string key);

        public void Remove(string key, bool fromFile = true, Action<bool> callback = null);

        public void Clear(bool fromFile = true, Action<bool> callback = null);
        public void Clear(string keyExclude, bool fromFile = true, Action<bool> callback = null);
        public void Clear(string[] keyExcludes, bool fromFile = true, Action<bool> callback = null);
    }
}
