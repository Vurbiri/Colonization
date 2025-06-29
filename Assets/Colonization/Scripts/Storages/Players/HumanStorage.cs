using System.Collections.Generic;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Storage
{
    using static CONST;
    using static SAVE_KEYS;

    sealed public class HumanStorage : APlayerStorage
    {
        private readonly Roads.Converter _roadsConverter = new();
        private readonly string _keyResources, _keyExchange, _keyRoads, _keyPerks;
        private readonly Dictionary<int, string[]> _keysEdifices = new(EdificeGroupId.Count);

        public HumanLoadData LoadData { get; set; }

        public HumanStorage(int id, IStorageService storage, bool isLoad) : base(id, storage)
        {
            _keyResources = P_RESOURCES.Concat(_strId); _keyExchange = P_EXCHANGE.Concat(_strId);
            _keyRoads = P_ROADS.Concat(_strId); _keyPerks = P_PERKS.Concat(_strId);

            var actors = InitActors(DEFAULT_MAX_ACTORS, isLoad);
            var edifices = CreateEdificesLoadData(isLoad);

            if (isLoad) 
                LoadData = new(storage.Get<int[]>(_keyResources), storage.Get<int[]>(_keyExchange), storage.Get<int[]>(_keyArtefact), storage.Get<int[][]>(_keyPerks), edifices, actors);
            else        
                LoadData = new();

            #region Local: CreateEdificesLoadData(..)
            //================================================================
            Dictionary<int, List<EdificeLoadData>> CreateEdificesLoadData(bool isLoad)
            {
                Dictionary<int, List<EdificeLoadData>> edifices = new(EdificeGroupId.Count);
                List<EdificeLoadData> loadData; string[] keys;
                string key, strI; bool load; EdificeLoadData data = null;
                for (int i = 0; i < EdificeGroupId.Count; i++)
                {
                    strI = i.ToString(); load = isLoad;
                    loadData = new(DEFAULT_MAX_EDIFICES); keys = new string[DEFAULT_MAX_EDIFICES];
                    for (int j = 0; j < DEFAULT_MAX_EDIFICES; j++)
                    {
                        keys[j] = key = P_EDIFICES.Concat(_strId, strI, j.ToString());
                        if (load = (load && _storage.TryGet(key, out data)))
                            loadData.Add(data);
                    }
                    edifices[i] = loadData;
                    _keysEdifices[i] = keys;
                }
                return edifices;
            }
            #endregion
        }

        public bool PopulateRoads(Roads roads, Crossroads crossroads) => _storage.TryPopulate<Roads>(_keyRoads, new Roads.Converter(roads, crossroads));
        public void BindRoads(IReactive<Roads> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(value => _storage.Save(_keyRoads, value, _roadsConverter), instantGetValue);
        }

        public void BindCurrencies(IReactive<ACurrencies> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(exchange => _storage.Set(_keyResources, exchange), instantGetValue);
        }

        public void BindExchange(IReactive<ACurrencies> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(currencies => _storage.Set(_keyExchange, currencies), instantGetValue);
        }

        public void BindPerks(IReactive<HashSet<int>[]> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(perks => _storage.Set(_keyPerks, perks), instantGetValue);
        }

        public void BindEdifices(IReadOnlyList<IReactiveList<Crossroad>> edificesReactive, bool instantGetValue)
        {
            for(int i = 0; i < EdificeGroupId.Count; i++)
                _unsubscribers += edificesReactive[i].Subscribe(OnEdifice, instantGetValue);

            #region Local OnEdifice(..)
            //==============================
            void OnEdifice(int index, Crossroad crossroad, TypeEvent operation)
            {
                string key = _keysEdifices[crossroad.GroupId.Value][index];
                _storage.Save(key, crossroad);
            }
            #endregion
        }

        protected override string GetNewKey(int index) => _keysActors[index];
    }
}
