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
        private int[] _perks;

        private readonly string _keyResources, _keyEdifices, _keyRoads, _keyPerks;

        public HumanLoadData LoadData { get; }

        public HumanSaveData(int id, IStorageService storage, bool isLoad) : base(id, storage, isLoad)
        {
            string strId = id.ToString();
            _keyResources = P_RESOURCES.Concat(strId); _keyEdifices = P_EDIFICES.Concat(strId); _keyRoads = P_ROADS.Concat(strId);
            _keyPerks = P_PERKS.Concat(strId);

            if (!(isLoad && storage.TryGet(_keyResources, out int[] resources)))
                resources = new int[CurrencyId.CountAll];

            if (!(isLoad && storage.TryGet(_keyEdifices, out _edifices)))
            {
                _edifices = new(EdificeGroupId.Count);
                for (int i = 0; i < EdificeGroupId.Count; i++)
                    _edifices[i] = new();
            }

            if (!(isLoad && storage.TryGet(_keyRoads, out int[][][] roads)))
                roads = new int[0][][];

            if (!(isLoad && storage.TryGet(_keyBuffs, out int[] buffs)))
                buffs = new int[0];

            if (!(isLoad && storage.TryGet(_keyPerks, out _perks)))
                _perks = new int[0];

            if(isLoad)
                LoadData = new(resources, _edifices, roads, buffs, _actors);
        }
                

        public void CurrenciesBind(IReactive<IReadOnlyList<int>> currencies, bool calling)
        {
            _unsubscribers += currencies.Subscribe(value => _storage.Save(_keyResources, value, DELAY_SAVE), calling);
        }

        public void EdificesBind(IReadOnlyList<IReactiveList<IArrayable>> edificesReactive)
        {
            for(int i = 0; i < EdificeGroupId.Count; i++)
                Bind(edificesReactive[i], _edifices[i]);

            #region Local Bind(..)
            //==============================
            void Bind(IReactiveList<IArrayable> edificesReactive, List<int[]> edifices)
            {
                _unsubscribers += edificesReactive.Subscribe(OnEdifice, false);

                #region Local OnEdifice(..)
                //==============================
                void OnEdifice(int index, IArrayable crossroad, TypeEvent operation)
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

                    _storage.Save(_keyEdifices, _edifices, DELAY_SAVE);
                }
                #endregion
            }
            #endregion
        }
        public void RoadsBind(IReactive<int[][][]> roadsReactive)
        {
            _unsubscribers += roadsReactive.Subscribe(value => _storage.Save(_keyRoads, value, DELAY_SAVE), false);
        }
    }
}
