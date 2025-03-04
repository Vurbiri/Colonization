//Assets\Colonization\Scripts\Data\ProjectSaveData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    public class ProjectSaveData : IDisposable
    {
        //private PlayersSaveData _playersSaveData;
        private PlayerSaveData[] _playersSaveData;
        private readonly IStorageService _storage;
        private Unsubscribers _unProject = new(), _unGameplay = new();

        public ProjectSaveData(IStorageService storage)
        {
            _storage = storage;
        }

        public bool Load;

        public PlayerSaveData[] PlayersSaveData
        {
            get
            {
               if (_playersSaveData != null) return _playersSaveData;

                _playersSaveData = new PlayerSaveData[PlayerId.PlayersCount];
                for (int i = 0; i < PlayerId.PlayersCount; i++)
                    _playersSaveData[i] = new(i, _storage, Load);

                return _playersSaveData;
            }
        }

        #region Load
        public bool TryGetSettingsData(out int[] profile, out float[] volumes)
        {
            return _storage.TryGet(SAVE_KEYS.SETTINGS_P, out profile) & _storage.TryGet(SAVE_KEYS.SETTINGS_V, out volumes);
        }

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

        public bool TryGetTurnQueueData(out int[] data)
        {
            data = null;
            return Load && _storage.TryGet(SAVE_KEYS.TURNS_QUEUE, out data);
        }
        #endregion

        #region Bind
        public void SettingsBind(IReactive<IReadOnlyList<int>, IReadOnlyList<float>> settings, bool calling)
        {
            _unProject += settings.Subscribe((p, v) => Save(p, v), calling);

            #region Local Save(..)
            //==============================
            void Save(IReadOnlyList<int> profile, IReadOnlyList<float> volumes)
            {
                _storage.Save(SAVE_KEYS.SETTINGS_P, profile, false);
                _storage.Save(SAVE_KEYS.SETTINGS_V, volumes);
            }
            #endregion
        }

        public void HexagonsBind(IReactive<Key, int[]> hex)
        {
            _unGameplay += hex.Subscribe((key, data) => _storage.Save(key.ToSaveKey(SAVE_KEYS.HEX_SEPARATOR), data, 1f));
        }
        public void DiplomacyBind(IReactive<IReadOnlyList<int>> diplomacy, bool calling)
        {
            _unGameplay += diplomacy.Subscribe(data => _storage.Save(SAVE_KEYS.DIPLOMANCY, data), calling);
        }

        public void TurnStateBind(TurnQueue turn, bool calling)
        {
            _unGameplay += turn.Subscribe(iTurn => _storage.Save(SAVE_KEYS.TURNS_QUEUE, iTurn.ToArray()), calling);
        }
        #endregion

        public void Dispose()
        {
            _unProject.Unsubscribe();
            _unGameplay.Unsubscribe();
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _playersSaveData[i].Dispose();
        }
    }
}
