using System;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class Crossroads : MonoBehaviour
{
    [SerializeField] private Crossroad _prefabCrossroad;

    public event Action<Crossroad> EventSelectCrossroad;

    private Transform _thisTransform;
    private Vector2 _offset;
    private Dictionary<Key, Crossroad> _crossroads;

    public void Initialize(int circleMax)
    {
        //Debug.Log($"Count Crossroads calk: {HEX_SIDE * circleMax * circleMax}");
        _crossroads = new(COUNT_SIDES * circleMax * circleMax);
        //Debug.Log($"Count Roads calk: {HEX_SIDE * circleMax * (circleMax - 1)}");
        //_crossLinks = new(COUNT_SIDES * circleMax * (circleMax - 1));

        _offset = new(HEX_RADIUS * COS_30, HEX_RADIUS * SIN_30);
        _thisTransform = transform;
    }

    public void CreateCrossroad(Vector3 position, Hexagon hex, bool isCircleMax)
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
                if (isCircleMax)
                    continue;

                cross = Instantiate(_prefabCrossroad, positionCross, Quaternion.identity, _thisTransform);
                cross.Initialize(key, SelectCrossroad);
                _crossroads.Add(key, cross);
            }

            cross.AddHexagon(hex);
            hex.Crossroads.Add(cross);
        }
    }

    public void CreateCrossroadLink(Hexagon hexA, Hexagon hexB)
    {
        if ((hexA.IsWater && hexB.IsWater) || (hexA.IsGate || hexB.IsGate))
            return;

        HashSet<Crossroad> cross = new(hexA.Crossroads);
        cross.IntersectWith(hexB.Crossroads);
        if (cross.Count != 2)
            return;

        Crossroad crossA, crossB;
        IEnumerator<Crossroad> enumerator = cross.GetEnumerator();
        crossA = GetCrossroad();
        crossB = GetCrossroad();

        new CrossroadLink(crossA, crossB);

        #region Local: GetCrossroad()
        //=================================
        Crossroad GetCrossroad()
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
        #endregion
    }

    public void Setup()
    {
        foreach (var crossroad in _crossroads.Values)
            crossroad.Setup();
    }

    private void SelectCrossroad(Crossroad cross)
    {
        EventSelectCrossroad?.Invoke(cross);
    }
}
