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
        private Unsubscribers _unProject = new(), _unGameplay = new();

        public ProjectSaveData(Coroutines coroutine, IStorageService storage)
        {
            _coroutines = coroutine;
            _storage = storage;
        }

        public bool Load;

        public PlayersSaveData PlayersSaveData => _playersSaveData ??= new(Load, _coroutines, _storage);

        #region Load
        public int[] SettingsLoadData => _storage.Get<int[]>(SAVE_KEYS.SETTINGS);

        public void GetHexData(Key key, out int id, out int surfaceId)
        {
            _storage.TryGet(key.ToSaveKey(SAVE_KEYS.HEX_SEPARATOR), out int[] data);
            Hexagon.FromArray(data, out id, out surfaceId);
        }

        public bool TryGetDiplomacyData(out int[] data)
        {
            data = null;
            return Load && _storage.TryGet(SAVE_KEYS.DIPLOMANCY, out data);
        }

        public bool TryGetTurnQueueData(out int[] queue, out int[] data)
        {
            queue = data = null;
            return Load && _storage.TryGet(SAVE_KEYS.TURNS_QUEUE, out queue) & _storage.TryGet(SAVE_KEYS.TURN_STATE, out data);
        }
        #endregion

        #region Bind
        public void SettingsBind(IReactive<IReadOnlyList<int>> settings)
        {
            _unProject += settings.Subscribe( data => _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.SETTINGS, data)));
        }

        public void HexagonsBind(IReactive<Key, int[]> hex)
        {
            _unGameplay += hex.Subscribe((key, data) => _coroutines.Run(_storage.Save_Cn(key.ToSaveKey(SAVE_KEYS.HEX_SEPARATOR), data)));
        }
        public void DiplomacyBind(IReactive<IReadOnlyList<int>> diplomacy, bool calling)
        {
            _unGameplay += diplomacy.Subscribe(data => _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.DIPLOMANCY, data)), calling);
        }

        public void SaveTurnQueue(IEnumerable<int> queue) => _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.TURNS_QUEUE, queue));
        public void TurnStateBind(TurnQueue turn, bool calling)
        {
            if(calling) _coroutines.Run(_storage.Save_Cn<IEnumerable<int>>(SAVE_KEYS.TURNS_QUEUE, turn));
            _unGameplay += turn.Subscribe(data => _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.TURN_STATE, data.ToArray())), calling);
        }
        #endregion

        public void Dispose()
        {
            _unProject.Unsubscribe();
            _unGameplay.Unsubscribe();
            _playersSaveData.Dispose();
        }
    }
}
