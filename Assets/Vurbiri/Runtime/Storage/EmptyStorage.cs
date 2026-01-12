using Newtonsoft.Json;
using System.Collections;

namespace Vurbiri
{
    public class EmptyStorage : IStorageService
    {
        public bool IsValid => false;
        public bool IsSaved => true;

        public IEnumerator Load_Cn(Out<bool> output)
        {
            output?.Set(false);
            return null;
        }

        public void Save() { }
        public IEnumerator Save(out WaitResult<bool> wait) => wait = WaitResult.Instant(false);
        public void Save<T>(string key, T data, JsonSerializerSettings settings = null) { }
        public void Save<T>(string key, T data, JsonConverter converter) { }

        public bool Set<T>(string key, T data, JsonSerializerSettings settings = null) => false;
        public bool Set<T>(string key, T data, JsonConverter converter) => false;

        public T Get<T>(string key) => default;
        public T Get<T>(string key, JsonConverter converter) => default;

        public T Extract<T>(string key) => default;

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            return false;
        }
        public bool TryGet<T>(string key, JsonConverter converter, out T value)
        {
            value = default;
            return false;
        }

        public bool TryPopulate(string key, object obj, JsonConverter converter = null) => false;
        public bool TryPopulate<T>(string key, JsonConverter converter) => false;

        public bool ContainsKey(string key) => false;

        public void Remove(string key, bool toFile) {}

        public void Clear() { }
        public void Clear(string keyExclude) { }
        public void Clear(params string[] keyExcludes) { }
    }
}
