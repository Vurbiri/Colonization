using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Crossroads
    {
        private readonly Dictionary<Key, Crossroad> _crossroads = new(MAX_CROSSROADS);

        private readonly HashSet<Crossroad> _breach = new(HEX.SIDES * (MAX_CIRCLES + HEX.SIDES));
        private readonly HashSet<Crossroad> _gate = new(HEX.SIDES);

        private Transform _container;
       
        private Vector3[] _vertices = new Vector3[HEX_COUNT_VERTICES];
        private Quaternion[] _angles = { Quaternion.Euler(0, 180, 0), Quaternion.identity };

        public Crossroad this[Key key] => _crossroads[key];

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
                    hex.Crossroads.Add(cross);
                else
                    _crossroads.Remove(key);

                if (ending)
                {
                    if (cross.IsBreach)
                        _breach.Add(cross);
                    else if(cross.IsGate)
                        _gate.Add(cross);
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
                if (i-- == 0) return breach;
            return null;
        }

        public void BindEdifices(IReadOnlyList<IReactiveList<Crossroad>> edificesReactive, bool instantGetValue)
        {
            edificesReactive[EdificeGroupId.Port].Subscribe(OnAddPort, instantGetValue);
            edificesReactive[EdificeGroupId.Shrine].Subscribe((_, cross, _) => _gate.Remove(cross), instantGetValue);
        }

        private void OnAddPort(int index, Crossroad crossroad, TypeEvent operation)
        {
            if (operation == TypeEvent.Add | operation == TypeEvent.Subscribe)
            {
                List<Hexagon> hexagons = crossroad.Hexagons;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                    if (hexagons[i].IsWater)
                        _breach.ExceptWith(hexagons[i].Crossroads);
            }
        }
    }
}
