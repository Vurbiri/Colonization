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
        public HexLoadData GetHexData(Key key) => _storage.Get<HexLoadData>(key.ToSaveKey());

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
        public void HexagonsBind(IReactive<Hexagon> reactive)
        {
            _unsubscribers += reactive.Subscribe(hex => _storage.Set(hex.Key.ToSaveKey(), hex), false);
        }
        public void DiplomacyBind(IReactive<IReadOnlyList<int>> reactive, bool calling)
        {
            _unsubscribers += reactive.Subscribe(diplomacy => _storage.Set(SAVE_KEYS.DIPLOMANCY, diplomacy), calling);
        }
        public void TurnQueueBind(IReactive<TurnQueue> reactive, bool calling)
        {
            _unsubscribers += reactive.Subscribe(turn => _storage.Set(SAVE_KEYS.TURNS_QUEUE, turn), calling);
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
