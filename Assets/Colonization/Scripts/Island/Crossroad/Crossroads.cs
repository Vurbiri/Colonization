//Assets\Colonization\Scripts\Island\Crossroad\Crossroads.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Crossroads
    {
        private readonly Transform _container;
        private readonly IdSet<EdificeId, AEdifice> _prefabs;
        private readonly GameplayTriggerBus _triggerBus;
        private readonly Dictionary<Key, Crossroad> _crossroads = new(HEX.SIDES * MAX_CIRCLES * MAX_CIRCLES);
        private readonly Vector3[] _vertices = new Vector3[HEX_COUNT_VERTICES];
        private readonly Quaternion ANGLE_0 = Quaternion.identity, ANGLE_180 = Quaternion.Euler(0, 180, 0);

        public Crossroad this[Key key] => _crossroads[key];

        public Crossroads(Transform container, IdSet<EdificeId, AEdifice> prefabs, GameplayTriggerBus triggerBus)
        {
            _container = container;
            _prefabs = prefabs;
            _triggerBus = triggerBus;

            for (int i = 0; i < HEX_COUNT_VERTICES; i++)
                _vertices[i] = HEX_RADIUS_OUT * VERTEX_DIRECTIONS[i];
        }

        public void CreateCrossroads(Vector3 positionHex, Hexagon hex, bool isLastCircle)
        {
            Crossroad cross;
            Key key;
            Vector3 position;
            for (int i = 0; i < HEX.SIDES; i++)
            {
                position = _vertices[i] + positionHex;

                key = position.CrossPositionToKey();

                if (!_crossroads.TryGetValue(key, out cross))
                {
                    if (isLastCircle)
                        continue;

                    cross = new(key, _container, position, i % 2 == 0 ? ANGLE_180 : ANGLE_0, _prefabs, _triggerBus);
                    _crossroads.Add(key, cross);
                }

                if (cross.AddHexagon(hex))
                    hex.CrossroadAdd(cross);
                else
                    _crossroads.Remove(key);
            }
        }
    }
}
