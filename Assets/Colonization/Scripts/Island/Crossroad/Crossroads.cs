using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Crossroads : MonoBehaviour
    {
        [SerializeField] private Crossroad _prefabCrossroad;

        private Transform _thisTransform;
        private Dictionary<Key, Crossroad> _crossroads;

        private readonly Quaternion ANGLE_0 = Quaternion.identity, ANGLE_180 = Quaternion.Euler(0, 180, 0);

        public Crossroad this[Key key] => _crossroads[key];

        public void Init()
        {
            _crossroads = new(HEX_COUNT_SIDES * MAX_CIRCLES * MAX_CIRCLES);
            _thisTransform = transform;
        }

        public void CreateCrossroads(Vector3 position, Hexagon hex, bool isLastCircle)
        {
            Crossroad cross;
            Key key;
            Vector3 positionCross;
            for (int i = 0; i < HEX_COUNT_SIDES; i++)
            {
                positionCross = HEX_VERTICES[i] + position;

                key = positionCross.CrossPositionToKey();

                if (!_crossroads.TryGetValue(key, out cross))
                {
                    if (isLastCircle)
                        continue;

                    cross = Instantiate(_prefabCrossroad, positionCross, i % 2 == 0 ? ANGLE_180 : ANGLE_0, _thisTransform);
                    cross.Init(key);
                    _crossroads.Add(key, cross);
                }

                if (cross.AddHexagon(hex))
                    hex.CrossroadAdd(cross);
                else
                    _crossroads.Remove(key);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_prefabCrossroad == null)
                _prefabCrossroad = VurbiriEditor.Utility.FindAnyPrefab<Crossroad>();
        }
#endif
    }
}
