using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    public abstract class AStorage : IDisposable
    {
        protected readonly IStorageService _storage;
        protected Subscription _subscription;

        protected AStorage(IStorageService storage)
        {
            _storage = storage;
        }

        public T Get<T>(string key) => _storage.Get<T>(key);
        public bool TryGet<T>(string key, out T value) => _storage.TryGet<T>(key, out value);

        public bool Set<T>(string key, T data) => _storage.Set<T>(key, data);
        public void Save<T>(string key, T data) => _storage.Save<T>(key, data);

        public void Save(Action<bool> callback = null) => _storage.Save(callback);

        public void Clear() => _storage.Clear(PROFILE, VOLUMES, COLORS, GAME_SETTINGS);

        public virtual void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
