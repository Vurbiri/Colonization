using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Crossroads
    {
        private readonly Dictionary<Key, Crossroad> _crossroads = new(MAX_CROSSROADS);

        private readonly Breach _breach = new();

        private ReadOnlyArray<int> _hexWeight;
        private Transform _container;
       
        private Vector3[] _vertices = new Vector3[HEX_COUNT_VERTICES];
        private Quaternion[] _angles = { Quaternion.Euler(0, 180, 0), Quaternion.identity };

        public Crossroad this[Key key] { [Impl(256)] get => _crossroads[key]; }

        public int BreachCount => _breach.Count;

        public Crossroads(Transform container, IdSet<EdificeId, AEdifice> prefabs, ReadOnlyArray<int> hexWeight)
        {
            _container = container;
            _hexWeight = hexWeight;

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
                        cross.SetWeightAndLinks(_hexWeight);
                        _breach.Add(cross);
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
            _hexWeight = null;
            _vertices = null;
            _angles = null;

            _breach.TrimExcess();
        }

        public Crossroad GetRandomPort() => _crossroads[_breach.Get()];
    }
}
