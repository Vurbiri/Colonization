//Assets\Colonization\Scripts\Data\PlayersData\PlayersSaveData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    using static PlayerId;

    public class PlayersSaveData : IDisposable
    {
        private readonly PlayerSaveData[] _playersData = new PlayerSaveData[PlayersCount];
        private int[] _diplomacyData;
        private int[] _turnQueueData;

        private readonly string[] _keys = new string[PlayersCount];

        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;
        private IUnsubscriber _unsubscriber;

        public PlayerSaveData this[int index] => _playersData[index];
        public IReadOnlyList<int> DiplomacyData => _diplomacyData;

        public PlayersSaveData(bool isLoading)
        {
            _coroutines = SceneServices.Get<Coroutines>();
            _storage = SceneServices.Get<IStorageService>();

            PlayerSaveData data = null;
            if (!isLoading)
            {
                for (int i = 0; i < PlayersCount; i++)
                {
                    _keys[i] = SAVE_KEYS.PLAYERS.Concat(i);
                    data = new(i);
                    data.Subscribe(OnSave, false);
                    _playersData[i] = data;
                }

                return;
            }

            for (int i = 0; i < PlayersCount; i++)
            {
                if(!_storage.TryGet(_keys[i] = SAVE_KEYS.PLAYERS.Concat(i), out data))
                    data = new(i);

                data.Subscribe(OnSave, false);
                _playersData[i] = data;
            }

            _diplomacyData = _storage.Get<int[]>(SAVE_KEYS.DIPLOMANCY);
        }
                

        public void Save(bool saveToFile = true, Action<bool> callback = null)
        {
            
            _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.DIPLOMANCY, _diplomacyData, saveToFile, callback));
            for (int i = 0; i < PlayersCount; i++)
                _coroutines.Run(_storage.Save_Cn(_keys[i], _playersData[i], saveToFile, callback));
        }

        public void DiplomacyBind(IReactive<int, int> diplomacy, bool calling)
        {
            _diplomacyData ??= new int[PlayersCount];
            _unsubscriber = diplomacy.Subscribe(OnDiplomacy, calling);

            #region Local OnDiplomacy(...)
            //==============================
            void OnDiplomacy(int index, int value)
            {
                _diplomacyData[index] = value;
                _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.DIPLOMANCY, _diplomacyData));
            }
            #endregion
        }

        public bool TryLoadTurnQueue(out int[] queue, out int[] data)
        {
            if (!(_storage.TryGet(SAVE_KEYS.TURNS_QUEUE, out queue) & _storage.TryGet(SAVE_KEYS.TURNS_DATA, out data)))
                return false;

            _turnQueueData = data;
            return true;
        }
        public void SaveTurnQueue(IEnumerable<int> queue) => _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.TURNS_QUEUE, queue));
        public void TurnQueueBind(IReadOnlyReactive<IArrayable> turnData)
        {
            _turnQueueData ??= turnData.Value.ToArray();
            _unsubscriber = turnData.Subscribe(OnTurnQueue, true);

            #region Local OnDiplomacy(...)
            //==============================
            void OnTurnQueue(IArrayable item)
            {
                _turnQueueData = item.ToArray(_turnQueueData);
                _coroutines.Run(_storage.Save_Cn(SAVE_KEYS.TURNS_DATA, _turnQueueData));
            }
            #endregion
        }

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();

            foreach (var player in _playersData)
                player.Dispose();
        }

        private void OnSave(PlayerSaveData data) => _coroutines.Run(_storage.Save_Cn(_keys[data.Id], data));
    }
}
