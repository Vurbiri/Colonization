//Assets\Colonization\Scripts\Data\PlayersData\DemonsSaveData.cs

namespace Vurbiri.Colonization.Data
{
    using Vurbiri.Reactive;
    using static SAVE_KEYS;

    public class SatanSaveData : APlayerSaveData
    {
        private int[] _status;
        
        public SatanLoadData LoadData { get; set; }

        public SatanSaveData(IStorageService storage, bool isLoad) : base(PlayerId.Satan, storage, isLoad)
        {
            if (!(isLoad && storage.TryGet(P_SATAN, out _status)))
                _status = new int[Satan.SIZE_ARRAY];

            if (isLoad)
                LoadData = new(storage.Get<int[]>(_keyArtefact), _status, _actors);
        }

        public void StatusBind(IReactive<Satan> status, bool calling)
        {
            _unsubscribers += status.Subscribe(satan => _storage.Set(P_SATAN, _status = satan.ToArray(_status)), calling);
        }
    }
}
