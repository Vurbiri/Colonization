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
        private readonly IdHashSet<EdificeId, AEdifice> _prefabs;
        private readonly GameplayEventBus _eventBus;
        private readonly Dictionary<Key, Crossroad> _crossroads = new(HEX.SIDES * MAX_CIRCLES * MAX_CIRCLES);
        private readonly Quaternion ANGLE_0 = Quaternion.identity, ANGLE_180 = Quaternion.Euler(0, 180, 0);

        public Crossroad this[Key key] => _crossroads[key];

        public Crossroads(Transform container, IdHashSet<EdificeId, AEdifice> prefabs, GameplayEventBus eventBus)
        {
            _container = container;
            _prefabs = prefabs;
            _eventBus = eventBus;
        }

        public void CreateCrossroads(Vector3 position, Hexagon hex, bool isLastCircle)
        {
            Crossroad cross;
            Key key;
            Vector3 positionCross;
            for (int i = 0; i < HEX.SIDES; i++)
            {
                positionCross = HEX_VERTICES[i] + position;

                key = positionCross.CrossPositionToKey();

                if (!_crossroads.TryGetValue(key, out cross))
                {
                    if (isLastCircle)
                        continue;

                    cross = new(key, _container, positionCross, i % 2 == 0 ? ANGLE_180 : ANGLE_0, _prefabs, _eventBus);
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
