//Assets\Colonization\Scripts\Data\PlayersData\DemonsSaveData.cs
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    using static SAVE_KEYS;

    public class SatanSaveData : APlayerSaveData
    {
        private readonly int[] _status;
        
        public SatanLoadData LoadData { get; set; }

        public SatanSaveData(IStorageService storage, bool isLoad) : base(PlayerId.Satan, storage)
        {
            
            if (!(isLoad && storage.TryGet(P_SATAN, out _status)))
                _status = new int[Satan.SIZE_ARRAY];
            SatanState state = new(_status);

            List<ActorLoadData> actors = InitActors(state.maxDemons, isLoad);

            if (isLoad) LoadData = new(storage.Get<int[]>(_keyArtefact), state, actors);
            else LoadData = new();
        }

        public void StatusBind(IReactive<Satan> status, bool calling)
        {
            _unsubscribers += status.Subscribe(satan => _storage.Set(P_SATAN, satan.CopyToArray(_status)), calling);
        }

        protected override string GetNewKey(int index)
        {
            for (int i = _keysActors.Count - 1; i <= index; i++)
                _keysActors.Add(P_ACTORS.Concat(_strId, i.ToString()));

            return _keysActors[index];
        }
    }
}
