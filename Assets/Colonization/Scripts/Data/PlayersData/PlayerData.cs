using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Data
{
    using static JSON_KEYS;

    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerData : /*AReactive<void>,*/ IDisposable
    {
        [JsonProperty(P_RESURSES)]
        private readonly int[] _resources;
        [JsonProperty(P_EDIFICES)]
        private List<int[]> _edifices;
        [JsonProperty(P_ROADS)]
        private int[][][] _roads;
        [JsonProperty(P_PERKS)]
        private int[] _perks;
        
        private readonly List<Unsubscriber<int>> _unsubscribersResources = new(CurrencyId.CountAll);
        private Unsubscriber<List<int[]>> _unsubscriberEdifices;
        private Unsubscriber<int[][][]> _unsubscriberRoads;

        public bool IsLoad { get; set; } = false;

        public int[] Resources => _resources;
        public List<int[]> Edifices => _edifices;
        public int[][][] Roads => _roads;
        public int[] Perks => _perks;

        public PlayerData()
        {
            _resources = new int[CurrencyId.CountAll];
            _perks = new int[0];
        }

        [JsonConstructor]
        public PlayerData(int[] resources, List<int[]> edifices, int[][][] roads, int[] perks)
        {
            _resources = resources;
            _edifices = edifices;
            _roads = roads;
            _perks = perks;
        }

        public void CurrenciesBind(AReadOnlyCurrenciesReactive currencies, bool calling)
        {
            for (int i = 0; i < CurrencyId.Count; i++)
                _unsubscribersResources[i] = currencies.Subscribe(i, (v) => _resources[i] = v, calling);
        }

        public void EdificesBind(IReactive<List<int[]>> edificesReactive, bool calling)
        { 
              _unsubscriberEdifices = edificesReactive.Subscribe(v => _edifices = v, calling);
        }

        public void RoadsBind(IReactive<int[][][]> roadsReactive, bool calling) => _unsubscriberRoads = roadsReactive.Subscribe(v => _roads = v, calling);


        public void Dispose()
        {
            foreach (var unsubscriber in _unsubscribersResources)
                unsubscriber?.Unsubscribe();
            
            _unsubscriberEdifices?.Unsubscribe();
            _unsubscriberRoads?.Unsubscribe();
        }

    }
}
