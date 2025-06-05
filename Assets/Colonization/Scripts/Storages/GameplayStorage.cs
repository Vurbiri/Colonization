using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    sealed public class GameplayStorage : AStorage
    {
        private readonly HumanStorage[] _humanStorages = new HumanStorage[PlayerId.HumansCount];
        private readonly SatanStorage _satanStorage;
        private readonly bool _isLoad;

        public GameplayStorage(bool isLoad) : base(SceneContainer.Get<IStorageService>())
        {
            _isLoad = isLoad;

            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humanStorages[i] = new(i, _storage, isLoad);

            _satanStorage = new(_storage, isLoad);
        }

        public bool Load => _isLoad;
        public HumanStorage[] Humans => _humanStorages;
        public SatanStorage Satan => _satanStorage;

        public void SaveGame(GameLoop game) => _storage.Save(SAVE_KEYS.GAME, game);

        #region Load
        public bool TryGetGame(out GameLoop game)
        {
            game = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.GAME, out game);
        }
        
        public int[] GetScoreData(int defaultSize)
        {
            if (_isLoad && _storage.TryGet(SAVE_KEYS.SCORE, out int[] data))
                return data;

            return new int[defaultSize];
        }
        public int GetBalanceValue(int defaultValue)
        {
            if (_isLoad && _storage.TryGet(SAVE_KEYS.BALANCE, out int value))
                return value;

            return defaultValue;
        }
        public bool TryGetDiplomacyData(out int[] data)
        {
            data = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.DIPLOMANCY, out data);
        }

        public HexLoadData GetHexData(Key key) => _storage.Get<HexLoadData>(key.ToSaveKey());
        #endregion

        #region Bind
        public void BindScore(IReactive<int[]> reactive)
        {
            _unsubscribers += reactive.Subscribe(scoreData => _storage.Set(SAVE_KEYS.SCORE, scoreData), !_isLoad);
        }
        public void BindBalance(IReactive<int> reactive)
        {
            _unsubscribers += reactive.Subscribe(balance => _storage.Set(SAVE_KEYS.BALANCE, balance), !_isLoad);
        }
        public void BindDiplomacy(IReactive<int[]> reactive)
        {
            _unsubscribers += reactive.Subscribe(diplomacyData => _storage.Set(SAVE_KEYS.DIPLOMANCY, diplomacyData), !_isLoad);
        }

        public void BindHexagons(IReactive<Hexagon> reactive)
        {
            _unsubscribers += reactive.Subscribe(hex => _storage.Set(hex.Key.ToSaveKey(), hex), false);
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humanStorages[i].Dispose();
        }
    }
}
