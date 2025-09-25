using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    sealed public class SatanStorage : APlayerStorage
    {
        public SatanLoadData LoadData { get; set; }

        public SatanStorage(IStorageService storage, bool isLoad) : base(PlayerId.Satan, storage, CONST.DEFAULT_MAX_DEMONS)
        {
            if (!(isLoad && storage.TryGet(P_SATAN, out SatanLoadState state)))
                state = new();

            List<ActorLoadData> actors = InitActors(CONST.DEFAULT_MAX_DEMONS, isLoad);

            if (isLoad) LoadData = new(storage.Get<int[]>(_keyArtefact), state, actors);
            else        LoadData = new(state);
        }

        public void StateBind(IReactive<Satan> reactive, bool instantGetValue)
        {
            _subscription += reactive.Subscribe(satan => _storage.Set(P_SATAN, satan), instantGetValue);
        }
    }
}
