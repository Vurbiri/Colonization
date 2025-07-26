using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    sealed public class SatanStorage : APlayerStorage
    {
        public SatanLoadData LoadData { get; set; }

        public SatanStorage(IStorageService storage, bool isLoad) : base(PlayerId.Satan, storage)
        {
            if (!(isLoad && storage.TryGet(P_SATAN, out SatanLoadState state)))
                state = new();

            List<ActorLoadData> actors = InitActors(CONST.DEFAULT_MAX_DEMONS, isLoad);

            if (isLoad) LoadData = new(storage.Get<int[]>(_keyArtefact), state, actors);
            else        LoadData = new(state);
        }

        public void StateBind(IReactive<Satan> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(satan => _storage.Set(P_SATAN, satan), instantGetValue);
        }

        protected override string GetNewKey(int index)
        {
            for (int i = _keysActors.Count; i <= index; i++)
                _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));

            return _keysActors[index];
        }
    }
}
