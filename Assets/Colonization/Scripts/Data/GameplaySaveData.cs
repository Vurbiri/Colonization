//Assets\Colonization\Scripts\Data\GameplaySaveData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    public class GameplaySaveData : IDisposable
	{
        private readonly HumanSaveData[] _humansSaveData = new HumanSaveData[PlayerId.HumansCount];
        private readonly SatanSaveData _satanSaveData;
        private readonly IStorageService _storage;
        private readonly bool _isLoad;
        private Unsubscribers _unsubscribers = new();

        public GameplaySaveData(bool isLoad)
        {
            _storage = SceneServices.Get<IStorageService>();
            _isLoad = isLoad;

            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humansSaveData[i] = new(i, _storage, isLoad);

            _satanSaveData = new(_storage, isLoad);
        }

        public bool Load => _isLoad;
        public HumanSaveData[] Humans => _humansSaveData;
        public SatanSaveData Satan => _satanSaveData;

        public void Save() => _storage.Save();

        #region Load
        public void GetHexData(Key key, out int id, out int surfaceId)
        {
            _storage.TryGet(key.ToSaveKey(), out int[] data);
            Hexagon.FromArray(data, out id, out surfaceId);
        }

        public bool TryGetDiplomacyData(out int[] data)
        {
            data = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.DIPLOMANCY, out data);
        }

        public bool TryGetTurnQueue(out TurnQueue turn)
        {
            turn = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.TURNS_QUEUE, out turn);
        }
        #endregion

        #region Bind
        public void HexagonsBind(IReactive<Key, int[]> hex)
        {
            _unsubscribers += hex.Subscribe((key, data) => _storage.Set(key.ToSaveKey(), data));
        }
        public void DiplomacyBind(IReactive<IReadOnlyList<int>> diplomacy, bool calling)
        {
            _unsubscribers += diplomacy.Subscribe(data => _storage.Set(SAVE_KEYS.DIPLOMANCY, data), calling);
        }
        public void TurnQueueBind(TurnQueue turn, bool calling)
        {
            _unsubscribers += turn.Subscribe(value => _storage.Set(SAVE_KEYS.TURNS_QUEUE, value), calling);
        }
        #endregion

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humansSaveData[i].Dispose();
        }
    }
}
