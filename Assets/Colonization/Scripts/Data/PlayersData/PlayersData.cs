using System;

namespace Vurbiri.Colonization.Data
{
    using static PlayerId;

    public class PlayersData : IDisposable
    {
        private readonly PlayerData[] _dataValues;
        private readonly string[] _keys;
        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;

        public PlayerData this[int index] => _dataValues[index];
        
        public PlayersData(bool isLoading)
        {
            _dataValues = new PlayerData[CountPlayers];
            _keys = new string[CountPlayers];

            _coroutines = SceneServices.Get<Coroutines>();
            _storage = SceneServices.Get<IStorageService>();

            PlayerData data = null;
            string key; bool isLoad;
            for (int i = 0; i < CountPlayers; i++)
            {
                _keys[i] = key = SAVE_KEYS.PLAYERS.Concat(i);
                isLoad = isLoading && _storage.TryGet(key, out data);
                data ??= new();
                data.IsLoad = isLoad;
                _dataValues[i] = data;
            }
        }

        public void Save(int id, bool saveToFile, Action<bool> callback = null)
        {
            _coroutines.Run(_storage.Save_Coroutine(_keys[id], _dataValues[id], saveToFile, callback));
        }

        public void Save(bool saveToFile, Action<bool> callback = null)
        {
            for (int i = 0; i < CountPlayers; i++)
                _coroutines.Run(_storage.Save_Coroutine(_keys[i], _dataValues[i], saveToFile, callback));
        }

        public void Dispose()
        {
            foreach (var player in _dataValues)
                player.Dispose();
        }
    }
}
