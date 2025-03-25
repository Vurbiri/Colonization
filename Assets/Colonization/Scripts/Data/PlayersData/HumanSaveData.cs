//Assets\Colonization\Scripts\Data\PlayersData\HumanSaveData.cs
using System.Collections.Generic;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Data
{
    using static SAVE_KEYS;

    public class HumanSaveData : APlayerSaveData
    {
        private readonly Dictionary<int, List<int[]>> _edifices;

        private readonly string _keyResources, _keyEdifices, _keyRoads, _keyPerks;

        public HumanLoadData LoadData { get; set; }

        public HumanSaveData(int id, IStorageService storage, bool isLoad) : base(id, storage, isLoad)
        {
            string strId = id.ToString();
            _keyResources = P_RESOURCES.Concat(strId); _keyEdifices = P_EDIFICES.Concat(strId); _keyRoads = P_ROADS.Concat(strId);
            _keyPerks = P_PERKS.Concat(strId);

            if (!(isLoad && storage.TryGet(_keyEdifices, out _edifices)))
            {
                _edifices = new(EdificeGroupId.Count);
                for (int i = 0; i < EdificeGroupId.Count; i++)
                    _edifices[i] = new();
            }

            if (isLoad) LoadData = new(storage.Get<int[]>(_keyResources), storage.Get<int[][][]>(_keyRoads), storage.Get<int[]>(_keyArtefact),
                                       storage.Get<int[][]>(_keyPerks), _edifices, _actors);
            else        LoadData = new();

        }

        public void CurrenciesBind(IReactive<IReadOnlyList<int>> currencies, bool calling)
        {
            _unsubscribers += currencies.Subscribe(value => _storage.Set(_keyResources, value), calling);
        }

        public void PerksBind(IReactive<IEnumerable<IEnumerable<int>>> eventPerks, bool calling)
        {
            _unsubscribers += eventPerks.Subscribe(perks => _storage.Set(_keyPerks, perks), calling);
        }

        public void EdificesBind(IReadOnlyList<IReactiveList<Crossroad>> edificesReactive)
        {
            for(int i = 0; i < EdificeGroupId.Count; i++)
                Bind(edificesReactive[i], _edifices[i]);

            #region Local Bind(..)
            //==============================
            void Bind(IReactiveList<Crossroad> edificesReactive, List<int[]> edifices)
            {
                _unsubscribers += edificesReactive.Subscribe(OnEdifice);

                #region Local OnEdifice(..)
                //==============================
                void OnEdifice(int index, Crossroad crossroad, TypeEvent operation)
                {
                    switch (operation)
                    {
                        case TypeEvent.Add:
                            edifices.Add(crossroad.ToArray());
                            break;
                        case TypeEvent.Remove:
                            edifices.RemoveAt(index);
                            break;
                        case TypeEvent.Change:
                            edifices[index] = crossroad.ToArray(edifices[index]);
                            break;
                        default:
                            return;
                    }

                    _storage.Set(_keyEdifices, _edifices);
                }
                #endregion
            }
            #endregion
        }
        public void RoadsBind(IReactive<Roads> roadsReactive, bool calling)
        {
            _unsubscribers += roadsReactive.Subscribe(value => _storage.Save(_keyRoads, value), calling);
        }
    }
}
