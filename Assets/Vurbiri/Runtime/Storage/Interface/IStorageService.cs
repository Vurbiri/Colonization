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

        public IEnumerator Save_Cn<T>(string key, T data, bool toFile = true, Action<bool> callback = null);

        public bool TryGet<T>(string key, out T value);

        public T Get<T>(string key) where T : class;

        public bool ContainsKey(string key);

        public IEnumerator Remove_Cn(string key, bool fromFile = true, Action<bool> callback = null);

        public IEnumerator Clear_Cn(bool fromFile = true, Action<bool> callback = null);
        public IEnumerator Clear_Cn(string keyExclude, bool fromFile = true, Action<bool> callback = null);
        public IEnumerator Clear_Cn(string[] keyExcludes, bool fromFile = true, Action<bool> callback = null);
    }
}
