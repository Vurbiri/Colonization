//Assets\Colonization\Scripts\Island\Hexagon\Hexagons.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Hexagons : IReactive<Key, int[]>
    {
        private readonly GameplayEventBus _eventBus;
        private readonly Pool<HexagonMark> _poolMarks;
        private readonly Dictionary<Key, Hexagon> _hexagons = new(MAX_HEXAGONS);
        private readonly Dictionary<int, List<Key>> _hexagonsIdForKey = new(HEX_IDS.Count + 1);

        private readonly Subscriber<Key, int[]> _subscriber = new();

        private LandMesh _landMesh;
        private Hexagon _prefabHex;
        private SurfacesScriptable _surfaces;
        private Transform _container;

        public Hexagon this[Key key] => _hexagons[key];

        public Hexagons(LandInitData initData, GameplayEventBus eventBus)
        {
            _landMesh = initData.landMesh.Init();
            _prefabHex = initData.prefabHex;
            _surfaces = initData.surfaces;
            _eventBus = eventBus;
            _container = _landMesh.transform;

            _poolMarks = new(initData.prefabHexMark, _container, HEX.SIDES);

            int count = HEX_IDS.Count, capacity = MAX_HEXAGONS / count + 1;
            for (int i = 0; i < count; i++)
                _hexagonsIdForKey[HEX_IDS[i]] = new List<Key>(capacity);
            _hexagonsIdForKey[GATE_ID] = new List<Key>(1);
        }

        public Hexagon CreateHexagon(Key key, int id, int surfaceId, Vector3 position)
        {
            SurfaceType surface = _surfaces[surfaceId];
            Hexagon hex = Object.Instantiate(_prefabHex, position, Quaternion.identity, _container);
            hex.Init(key, id, _poolMarks, surface,  _eventBus);

            _subscriber.Invoke(key, hex.ToArray());

            _hexagons.Add(key, hex);
            _hexagonsIdForKey[id].Add(key);
 
            _landMesh.AddHexagon(key, position, surfaceId);

            return hex;
        }

        public IEnumerator HexagonsNeighbors_Cn() => _landMesh.HexagonsNeighbors_Cn(_hexagons);

        public IEnumerator FinishCreate_Cn()
        {
            yield return _landMesh.SetMesh_Cn();

            _landMesh.Dispose();
            _surfaces.Dispose();

            _landMesh = null;
            _prefabHex = null;
            _surfaces = null;
            _container = null;

            yield return null;
        }

        public CurrenciesLite GetFreeGroundResource(int id)
        {
            CurrenciesLite res = new();
            foreach (var key in _hexagonsIdForKey[id])
                if (_hexagons[key].TryGetFreeGroundResource(out int currencyId))
                    res.Increment(currencyId);

            return res;
        }

        public Unsubscriber Subscribe(Action<Key, int[]> action, bool calling = true) => _subscriber.Add(action);

        public void Dispose() => _subscriber.Dispose();
    }
}
