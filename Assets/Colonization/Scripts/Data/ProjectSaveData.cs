//Assets\Colonization\Scripts\Data\ProjectSaveData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    public class ProjectSaveData : IDisposable
    {
        private PlayersSaveData _playersSaveData;

        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;
        private Unsubscribers _unsubscribers = new();

        public ProjectSaveData(Coroutines coroutine, IStorageService storage)
        {
            _coroutines = coroutine;
            _storage = storage;
        }

        public int[] SettingsLoadData => _storage.Get<int[]>(SAVE_KEYS.SETTINGS);

        public int[] GetHexData(Key key) => _storage.Get<int[]>(key.ToSaveKey(SAVE_KEYS.HEX_SEPARATOR));

        public void SettingsBind(IReactive<IReadOnlyList<int>> settings)
        {
            _unsubscribers += settings.Subscribe( data => _coroutines.Run(_storage.Save_Coroutine(SAVE_KEYS.SETTINGS, data)));
        }

        public void LandBind(IReactive<Key, int[]> hex)
        {
            _unsubscribers += hex.Subscribe((key, data) => _coroutines.Run(_storage.Save_Coroutine(key.ToSaveKey(SAVE_KEYS.HEX_SEPARATOR), data)));
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            _playersSaveData.Dispose();
        }
    }
}
