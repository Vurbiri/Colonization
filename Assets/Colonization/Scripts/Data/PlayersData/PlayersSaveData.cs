//Assets\Colonization\Scripts\Data\PlayersData\PlayersSaveData.cs
using System;

namespace Vurbiri.Colonization.Data
{
    using static PlayerId;

    public class PlayersSaveData : IDisposable
    {
        private readonly PlayerSaveData[] _playersData = new PlayerSaveData[PlayersCount];

        private readonly string[] _keys = new string[PlayersCount];

        private readonly IStorageService _storage;

        public PlayerSaveData this[int index] => _playersData[index];

        public PlayersSaveData(bool isLoad, IStorageService storage)
        {
            _storage = storage;

            PlayerSaveData data = null;
            for (int i = 0; i < PlayersCount; i++)
            {
                _keys[i] = SAVE_KEYS.PLAYERS.Concat(i);

                if (!(isLoad && _storage.TryGet(_keys[i], out data)))
                    data = new(i);

                data.Subscribe(OnSave, false);
                _playersData[i] = data;
            }
        }

        public void Save(bool saveToFile = true, Action<bool> callback = null)
        {
            for (int i = 0; i < PlayersCount; i++)
                _storage.Save(_keys[i], _playersData[i], saveToFile, callback);
        }

        public void Dispose()
        {
            foreach (var player in _playersData)
                player.Dispose();
        }

        private void OnSave(PlayerSaveData data) => _storage.Save(_keys[data.Id], data);
    }
}
