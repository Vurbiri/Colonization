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
        
        public PlayersData(bool isLoading, out bool[] loads)
        {
            _dataValues = new PlayerData[PlayersCount];
            _keys = new string[PlayersCount];
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
        }

        public void Save(bool saveToFile, Action<bool> callback = null)
        {
            for (int i = 0; i < PlayersCount; i++)
                _coroutines.Run(_storage.Save_Coroutine(_keys[i], _dataValues[i], saveToFile, callback));
        }

        public void Dispose()
        {
            foreach (var player in _dataValues)
                player.Dispose();
        }

        private void OnSave(PlayerData data) => _coroutines.Run(_storage.Save_Coroutine(_keys[data.Id], data));
    }
}
