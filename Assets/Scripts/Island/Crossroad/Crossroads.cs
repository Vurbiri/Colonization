using System.Collections.Generic;
using UnityEngine;
using static CONST;

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
        _crossroads = new(COUNT_SIDES * circleMax * circleMax);
        //Debug.Log($"Count Roads calk: {HEX_SIDE * circleMax * (circleMax - 1)}");

        _offset = new(HEX_RADIUS * COS_30, HEX_RADIUS * SIN_30);
        _thisTransform = transform;
    }

    public void CreateCrossroad(Vector3 position, Hexagon hex, bool isLastCircle)
    {
        Crossroad cross;
        Key key;
        Vector3 positionCross;
        for (int i = 0; i < COUNT_SIDES; i++)
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

            if(cross.AddHexagon(hex))
                hex.CrossroadAdd(cross);
            else
                _crossroads.Remove(key);
        }
    }

    public void CreateCrossroadLink(Hexagon hexA, Hexagon hexB)
    {
        if ((hexA.IsWater && hexB.IsWater) || (hexA.IsGate || hexB.IsGate) || !hexA.IntersectWith(hexB, out HashSet<Crossroad> cross))
            return;

        Crossroad crossA, crossB;
        IEnumerator<Crossroad> enumerator = cross.GetEnumerator();
        crossA = GetCrossroad();
        crossB = GetCrossroad();

        new CrossroadLink(crossA, crossB, hexA.IsWater || hexB.IsWater);

        #region Local: GetCrossroad()
        //=================================
        Crossroad GetCrossroad()
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
        #endregion
    }
}
