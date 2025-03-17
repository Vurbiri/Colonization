//Assets\Colonization\Scripts\Data\ProjectSaveData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    public class ProjectSaveData : IDisposable
    {
        private readonly IStorageService _storage;
        private readonly string[] _notClear = { SAVE_KEYS.SETTINGS_P, SAVE_KEYS.SETTINGS_V };
        private Unsubscribers _unsubscribers = new();

        public ProjectSaveData(IStorageService storage)
        {
            _storage = storage;
        }

        public void Save() => _storage.Save();

        public void Clear() => _storage.Clear(_notClear);

        #region Load
        public bool TryGetSettingsData(out int[] profile, out float[] volumes)
        {
            return _storage.TryGet(SAVE_KEYS.SETTINGS_P, out profile) & _storage.TryGet(SAVE_KEYS.SETTINGS_V, out volumes);
        }
        #endregion

        #region Bind
        public void SettingsBind(IReactive<IReadOnlyList<int>, IReadOnlyList<float>> settings, bool calling)
        {
            _unsubscribers += settings.Subscribe((p, v) => Save(p, v), calling);

            #region Local Save(..)
            //==============================
            void Save(IReadOnlyList<int> profile, IReadOnlyList<float> volumes)
            {
                _storage.Set(SAVE_KEYS.SETTINGS_P, profile);
                _storage.Save(SAVE_KEYS.SETTINGS_V, volumes);
            }
            #endregion
        }
        #endregion

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
}
