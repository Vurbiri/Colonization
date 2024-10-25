using System;

namespace Vurbiri.Colonization.Data
{
    using static PlayerId;

    public class PlayersData
    {
        private readonly PlayerData[] _values;

        private readonly Coroutines _coroutines;
        private readonly IStorageService _storage;

        public PlayerData this[int index] => _values[index];

        public PlayersData(PricesScriptable prices, Roads[] roads, Crossroads crossroads, bool isLoading)
        {
            _values = new PlayerData[CountPlayers];

            _coroutines = SceneServices.Get<Coroutines>();
            _storage = SceneServices.Get<IStorageService>();

            if(isLoading && _storage.TryGet(SAVE_KEYS.PLAYERS, out PlayerLoadData[] LoadValues))
            {
                for(int i = 0; i < LoadValues.Length; i++)
                    _values[i] = new(i, prices, LoadValues[i], crossroads, roads[i]);
            }
            else
            {
                for(int i = 0; i < CountPlayers; i++)
                    _values[i] = new(prices, roads[i]);
            }
        }

        public void Save(bool saveToFile, Action<bool> callback = null)
                    => _coroutines.Run(_storage.Save_Coroutine(SAVE_KEYS.PLAYERS, _values, saveToFile, callback));
    }
}
