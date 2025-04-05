//Assets\Colonization\Scripts\Storages\GameplayStorage.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    public class GameplayStorage : IDisposable
	{
        private readonly HumanStorage[] _humanStorages = new HumanStorage[PlayerId.HumansCount];
        private readonly SatanStorage _satanStorage;
        private readonly IStorageService _storage;
        private readonly bool _isLoad;
        private Unsubscribers _unsubscribers = new();

        public GameplayStorage(bool isLoad)
        {
            _storage = SceneContainer.Get<IStorageService>();
            _isLoad = isLoad;

            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humanStorages[i] = new(i, _storage, isLoad);

            _satanStorage = new(_storage, isLoad);
        }

        public bool Load => _isLoad;
        public HumanStorage[] Humans => _humanStorages;
        public SatanStorage Satan => _satanStorage;

        public void Save() => _storage.SaveAll();

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
        public void DiplomacyBind(IReactive<IReadOnlyList<int>> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(diplomacy => _storage.Set(SAVE_KEYS.DIPLOMANCY, diplomacy), instantGetValue);
        }
        public void TurnQueueBind(IReactive<TurnQueue> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(turn => _storage.Set(SAVE_KEYS.TURNS_QUEUE, turn), instantGetValue);
        }
        #endregion

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humanStorages[i].Dispose();
        }
    }
}
