using Newtonsoft.Json;
using System.Collections;

namespace Vurbiri.Storage
{
    public interface IStorageService
    {
        public bool IsValid { get; }
        public bool IsSaved { get; }

        public IEnumerator Load_Cn(Out<bool> output);

        public T Get<T>(string key);
        public T Get<T>(string key, JsonConverter converter);
        public bool TryGet<T>(string key, out T value);
        public bool TryGet<T>(string key, JsonConverter converter, out T value);

        public T Extract<T>(string key);

        public bool TryPopulate(string key, object obj, JsonConverter converter = null);
        public bool TryPopulate<T>(string key, JsonConverter converter);

        public bool Set<T>(string key, T data, JsonSerializerSettings settings = null);
        public bool Set<T>(string key, T data, JsonConverter converter);

        public void Save();
        public IEnumerator Save(out WaitResult<bool> wait);
        public void Save<T>(string key, T data, JsonSerializerSettings settings = null);
        public void Save<T>(string key, T data, JsonConverter converter);

        public bool ContainsKey(string key);

        public void Remove(string key, bool fromFile = true);

        public void Clear();
        public void Clear(string excludeKey);
        public void Clear(params string[] excludeKeys);
    }
}
