//Assets\Colonization\Scripts\Data\PlayersData\PlayersData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    using static PlayerId;

    public class PlayersData : IDisposable
    {
        private readonly PlayerData[] _dataValues = new PlayerData[PlayersCount];
        private readonly int[] _diplomacyData = new int[PlayersCount];

        private readonly string[] _keys = new string[PlayersCount];

        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;
        private IUnsubscriber _unsubscriber;

        public PlayerData this[int index] => _dataValues[index];
        public IReadOnlyList<int> DiplomacyData => _diplomacyData;

        public PlayersData(bool isLoading, out bool[] loads, out bool isLoadDiplomacy)
        {
            loads = new bool[PlayersCount];

            _coroutines = SceneServices.Get<Coroutines>();
            _storage = SceneServices.Get<IStorageService>();

            PlayerData data = null;
            string key; bool isLoad;
            for (int i = 0; i < PlayersCount; i++)
            {
                _keys[i] = key = SAVE_KEYS.PLAYERS.Concat(i);

                isLoad = isLoading && _storage.TryGet(key, out data);
                if(!isLoad) 
                    data = new(i);
                loads[i] = isLoad;

                data.Subscribe(OnSave, false);
                _dataValues[i] = data;
            }

            isLoadDiplomacy = isLoading && _storage.TryGet(SAVE_KEYS.DIPLOMANCY, out _diplomacyData);
        }

        public int LoadCurrentPlayerId(bool isLoading, int defaultValue)
        {
            if(isLoading && _storage.TryGet(SAVE_KEYS.CURRENT_PLAYER, out int value))
                return value;

            return defaultValue;
        }

        public void Save(int currentPlayerId, bool saveToFile = true, Action<bool> callback = null)
        {
            _coroutines.Run(_storage.Save_Coroutine(SAVE_KEYS.CURRENT_PLAYER, currentPlayerId, saveToFile, callback));
            _coroutines.Run(_storage.Save_Coroutine(SAVE_KEYS.DIPLOMANCY, _diplomacyData, saveToFile, callback));
            for (int i = 0; i < PlayersCount; i++)
                _coroutines.Run(_storage.Save_Coroutine(_keys[i], _dataValues[i], saveToFile, callback));
        }

        public void DiplomacyBind(IReactive<int, int> currencies, bool calling)
        {
            _unsubscriber = currencies.Subscribe(OnDiplomacy, calling);

            #region Local OnDiplomacy(...)
            //==============================
            void OnDiplomacy(int index, int value)
            {
                _diplomacyData[index] = value;
                _coroutines.Run(_storage.Save_Coroutine(SAVE_KEYS.DIPLOMANCY, _diplomacyData));
            }
            #endregion
        }

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();

            foreach (var player in _dataValues)
                player.Dispose();
        }

        private void OnSave(PlayerData data) => _coroutines.Run(_storage.Save_Coroutine(_keys[data.Id], data));
    }
}
