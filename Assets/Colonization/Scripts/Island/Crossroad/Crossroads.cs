using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Crossroads
    {
        private readonly Dictionary<Key, Crossroad> _crossroads = new(MAX_CROSSROADS);

        private readonly HashSet<Key> _breach = new(HEX.SIDES * (MAX_CIRCLES + HEX.SIDES));
        private readonly HashSet<Key> _gate = new(HEX.SIDES);

        private Transform _container;
       
        private Vector3[] _vertices = new Vector3[HEX_COUNT_VERTICES];
        private Quaternion[] _angles = { Quaternion.Euler(0, 180, 0), Quaternion.identity };

        public Crossroad this[Key key] { [Impl(256)] get => _crossroads[key]; }

        public int BreachCount => _breach.Count;

        public Crossroads(Transform container, IdSet<EdificeId, AEdifice> prefabs)
        {
            _container = container;

            Crossroad.Init(prefabs);

            for (int i = 0; i < HEX_COUNT_VERTICES; i++)
                _vertices[i] = HEX_RADIUS_OUT * VERTEX_DIRECTIONS[i];
        }

        public void CrossroadCreate(Vector3 positionHex, Hexagon hex, bool isLastCircle)
        {
            Crossroad cross; Key key; Vector3 position;
            for (int i = 0; i < HEX.SIDES; i++)
            {
                position = _vertices[i] + positionHex;

                key = position.CrossPositionToKey();

                if (!_crossroads.TryGetValue(key, out cross))
                {
                    if (isLastCircle)
                        continue;

                    cross = new(key, _container, position, _angles[i % 2]);
                    _crossroads.Add(key, cross);
                }

                if (cross.AddHexagon(hex, out bool ending))
                {
                    hex.Crossroads.Add(cross);
                    if (ending)
                    {
                        if (cross.IsBreach)
                            _breach.Add(key);
                        else if (cross.IsGate)
                            _gate.Add(key);
                    }
                }
                else
                {
                    _crossroads.Remove(key);
                }
            }
        }

        public void FinishCreate()
        {
            _container = null;
            _vertices = null;
            _angles = null;

            _breach.TrimExcess();
        }

        public Crossroad GetRandomPort()
        {
            int i = UnityEngine.Random.Range(0, _breach.Count);
            foreach ( var breach in _breach )
                if (i-- == 0) return _crossroads[breach];
            return null;
        }

        [Impl(256)] public void RoadBuilt(Id<LinkId> id, Key start, Key end)
        {
            _crossroads[start].RoadBuilt(id);
            _crossroads[end].RoadBuilt(id);
        }
        [Impl(256)] public void RoadRemove(Id<LinkId> id, Key start, Key end)
        {
            _crossroads[start].RoadRemove(id);
            _crossroads[end].RoadRemove(id);
        }

        public void BindEdifices(IReadOnlyList<ReadOnlyReactiveList<Crossroad>> edificesReactive, bool instantGetValue)
        {
            edificesReactive[EdificeGroupId.Port].Subscribe(OnAddPort, instantGetValue);
            edificesReactive[EdificeGroupId.Shrine].Subscribe((_, cross, _) => _gate.Remove(cross.Key), instantGetValue);
        }

        private void OnAddPort(int index, Crossroad crossroad, TypeEvent operation)
        {
            if (operation == TypeEvent.Add | operation == TypeEvent.Subscribe)
            {
                var hexagons = crossroad.Hexagons;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                {
                    if (hexagons[i].IsWater)
                    {
                        var crossroads = hexagons[i].Crossroads;
                        for(int j = crossroads.Count - 1; j >= 0; j--)
                            _breach.Remove(crossroads[j].Key);
                    }
                }
            }
        }
    }
}
