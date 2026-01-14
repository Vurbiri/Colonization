using Vurbiri.Reactive;
using Vurbiri.Storage;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    sealed public class SatanStorage : APlayerStorage
    {
        public SatanLoadData LoadData { get; set; }

        public SatanStorage(IStorageService storage, bool isLoad) : base(PlayerId.Satan, storage)
        {
            if (isLoad)
            {
                if (!storage.TryGet(P_SATAN, out SatanLoadState state))
                    state = new();

                LoadData = new(storage.Get<int[]>(_keyArtefact), state, LoadActors(CONST.DEFAULT_MAX_DEMONS));
            }
            else
            {
                InitActors(CONST.DEFAULT_MAX_DEMONS);
                LoadData = new();
            }
        }

        public void StateBind(IReactive<Satan> reactive, bool instantGetValue)
        {
            _subscriptions += reactive.Subscribe(satan => _storage.Set(P_SATAN, satan), instantGetValue);
        }
    }
}
