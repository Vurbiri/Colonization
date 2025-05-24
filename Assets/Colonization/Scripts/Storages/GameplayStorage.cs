using System.Collections.Generic;
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

        #region Load
        public bool TryGetDiplomacyData(out int[] data)
        {
            data = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.DIPLOMANCY, out data);
        }

        public HexLoadData GetHexData(Key key) => _storage.Get<HexLoadData>(key.ToSaveKey());
        #endregion

        #region Bind
        public void BindDiplomacy(IReactive<IReadOnlyList<int>> reactive)
        {
            _unsubscribers += reactive.Subscribe(diplomacy => _storage.Set(SAVE_KEYS.DIPLOMANCY, diplomacy), !_isLoad);
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
