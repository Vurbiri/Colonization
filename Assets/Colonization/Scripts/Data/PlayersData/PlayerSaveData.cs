//Assets\Colonization\Scripts\Data\PlayersData\PlayerSaveData.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Data
{
    using static SAVE_KEYS;

    public class PlayerSaveData : IDisposable
    {
        private const float DELAY_SAVE = 0.5f;
        
        private readonly int[] _resources;
        private readonly Dictionary<int, List<int[]>> _edifices;
        private readonly List<int[][]> _warriors;
        private int[] _perks;

        private readonly string _keyResources, _keyEdifices, _keyRoads, _keyWarriors, _keyPerks;
        private readonly IStorageService _storage;
        private Unsubscribers _unsubscribers = new(CurrencyId.CountAll + EdificeGroupId.Count + 3);

        public PlayerLoadData LoadData { get; }

        public PlayerSaveData(int id, IStorageService storage, bool isLoad)
        {
            _storage = storage;

            string strId = id.ToString();
            _keyResources = P_RESOURCES.Concat(strId); _keyEdifices = P_EDIFICES.Concat(strId); _keyRoads = P_ROADS.Concat(strId);
            _keyWarriors = P_WARRIORS.Concat(strId); _keyPerks = P_PERKS.Concat(strId);

            if (!(isLoad && storage.TryGet(_keyResources, out _resources)))
                _resources = new int[CurrencyId.CountAll];

            if (!(isLoad && storage.TryGet(_keyEdifices, out _edifices)))
            {
                _edifices = new(EdificeGroupId.Count);
                for (int i = 0; i < EdificeGroupId.Count; i++)
                    _edifices[i] = new();
            }

            if (!(isLoad && storage.TryGet(_keyRoads, out int[][][] roads)))
                roads = new int[0][][];

            if (!(isLoad && storage.TryGet(_keyPerks, out _perks)))
                _perks = new int[0];

            if (!(isLoad && storage.TryGet(_keyWarriors, out _warriors)))
                _warriors = new();

            if(isLoad)
                LoadData = new(_resources, _edifices, roads, _warriors);
        }
                

        #region Bind
        public void CurrenciesBind(IReactive<int, int> currencies, bool calling)
        {
            _unsubscribers += currencies.Subscribe(OnCurrencies, calling);

            #region Local OnCurrencies(..)
            //==============================
            void OnCurrencies(int index, int value)
            {
                _resources[index] = value;
                _storage.Save(_keyResources, _resources, DELAY_SAVE);
            }
            #endregion
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
            _unsubscribers += roadsReactive.Subscribe(OnRoads, false);

            #region Local OnRoads(..)
            //==============================
            void OnRoads(int[][][] values)
            {
                _storage.Save(_keyRoads, values, DELAY_SAVE);
            }
            #endregion
        }
        public void WarriorsBind(IListReactiveItems<Actor> warriorsReactive)
        {
            _unsubscribers += warriorsReactive.Subscribe(OnWarriors, false);

            #region Local OnWarriors(..)
            //==============================
            void OnWarriors(Actor actor, TypeEvent operation)
            {
                switch (operation)
                {
                    case TypeEvent.Add:
                        _warriors.Add(actor.ToArray());
                        break;
                    case TypeEvent.Remove:
                        _warriors.RemoveAt(actor.Index);
                        break;
                    case TypeEvent.Change:
                        _warriors[actor.Index] = actor.ToArray(_warriors[actor.Index]);
                        break;
                    default:
                        return;
                }
                _storage.Save(_keyWarriors, _warriors, DELAY_SAVE);
            }
            #endregion
        }
        #endregion

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }

    }
}
