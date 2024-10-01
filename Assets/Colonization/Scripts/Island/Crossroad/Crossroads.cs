using System.Collections.Generic;
using UnityEngine;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public class Crossroads : MonoBehaviour
    {
        [SerializeField] private Crossroad _prefabCrossroad;

        private Transform _thisTransform;
        private Vector2 _offset;
        private Dictionary<Key, Crossroad> _crossroads;

        private static readonly Quaternion ANGLE_0 = Quaternion.identity, ANGLE_180 = Quaternion.Euler(0, 180, 0);

        public void Initialize(int circleMax)
        {
            //Debug.Log($"Count Crossroads calk: {HEX_SIDE * circleMax * circleMax}");
            _crossroads = new(HEX_COUNT_SIDES * circleMax * circleMax);
            //Debug.Log($"Count Roads calk: {HEX_SIDE * circleMax * (circleMax - 1)}");

            _offset = new(HEX_RADIUS_OUT * COS_30, HEX_RADIUS_OUT * SIN_30);
            _thisTransform = transform;
        }

        public void CreateCrossroad(Vector3 position, Hexagon hex, bool isLastCircle)
        {
            Crossroad cross;
            Key key;
            Vector3 positionCross;
            for (int i = 0; i < HEX_COUNT_SIDES; i++)
            {
                positionCross = HEX_VERTICES[i] + position;

                key = new(2f * positionCross.x / _offset.x, positionCross.z / _offset.y);

                if (!_crossroads.TryGetValue(key, out cross))
                {
                    if (isLastCircle)
                        continue;

                    cross = Instantiate(_prefabCrossroad, positionCross, i % 2 == 0 ? ANGLE_180 : ANGLE_0, _thisTransform);
                    cross.Initialize(key);
                    _crossroads.Add(key, cross);
                }

                if (cross.AddHexagon(hex))
                    hex.CrossroadAdd(cross);
                else
                    _crossroads.Remove(key);
            }
        }

        public Crossroad GetCrossroad(Key key) => _crossroads[key];

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_prefabCrossroad == null)
                _prefabCrossroad = VurbiriEditor.Utility.FindAnyPrefab<Crossroad>();
        }
#endif
    }
}
