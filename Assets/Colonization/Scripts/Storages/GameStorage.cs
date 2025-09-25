namespace Vurbiri.Colonization.Storage
{
    sealed public class GameStorage : AStorage
    {
        private readonly HumanStorage[] _humanStorages = new HumanStorage[PlayerId.HumansCount];
        private readonly SatanStorage _satanStorage;
        private readonly bool _isLoad;

        public GameStorage(bool isLoad) : base(GameContainer.StorageService)
        {
            _isLoad = isLoad;

            ContractResolver.Add(new GameLoop.Converter(), new Crossroad.Converter());

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
        
        public bool TryGetScore(out Score score)
        {
            score = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.SCORE, out score);
        }
        public int GetBalanceValue(int defaultValue)
        {
            if (_isLoad && _storage.TryGet(SAVE_KEYS.BALANCE, out int value))
                return value;

            return defaultValue;
        }
        public bool TryGetDiplomacy(out Diplomacy diplomacy)
        {
            diplomacy = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.DIPLOMANCY, out diplomacy);
        }

        public HexLoadData GetHexData(Key key) => _storage.Get<HexLoadData>(key.ToSaveKey());
        #endregion

        #region Bind
        public void BindScore(Score score)
        {
            _subscription += score.Subscribe(self => _storage.Set(SAVE_KEYS.SCORE, self), !_isLoad);
        }
        public void BindBalance(Balance balance)
        {
            _subscription += balance.Subscribe(balanceData => _storage.Set(SAVE_KEYS.BALANCE, balanceData), !_isLoad);
        }
        public void BindDiplomacy(Diplomacy diplomacy)
        {
            _subscription += diplomacy.Subscribe(self => _storage.Set(SAVE_KEYS.DIPLOMANCY, self), !_isLoad);
        }

        public void BindHexagons(Hexagons hexagons)
        {
            _subscription += hexagons.Subscribe(hex => _storage.Set(hex.Key.ToSaveKey(), hex), false);
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
