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

        public void Save(Action<bool> callback = null);
        public void Save<T>(string key, T data, Action<bool> callback = null);

        public bool Set<T>(string key, T data);

        public T Get<T>(string key) where T : class;
        public bool TryGet<T>(string key, out T value);

        public bool ContainsKey(string key);

        public void Remove(string key, bool fromFile = true, Action<bool> callback = null);

        public void Clear(Action<bool> callback = null);
        public void Clear(string excludeKey, Action<bool> callback = null);
        public void Clear(string[] excludeKeys, Action<bool> callback = null);
    }
}
