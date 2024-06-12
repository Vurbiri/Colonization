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
    private Dictionary<KeyDouble, CrossroadLink> _crossLinks;

    public void Initialize(int circleMax)
    {
        //Debug.Log($"Count Crossroads calk: {HEX_SIDE * circleMax * circleMax}");
        _crossroads = new(HEX_SIDE * circleMax * circleMax);
        //Debug.Log($"Count Roads calk: {HEX_SIDE * circleMax * (circleMax - 1)}");
        _crossLinks = new(HEX_SIDE * circleMax * (circleMax - 1));

        _offset = new(HEX_RADIUS * COS_30, HEX_RADIUS * SIN_30);
        _thisTransform = transform;
    }

    public void CreateCrossroad(Vector3 position, Hexagon hex, bool isCircleMax)
    {
        Crossroad cross;
        Key key;
        Vector3 positionCross;
        for (int i = 0; i < HEX_SIDE; i++)
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

    public bool CreateCrossroadLink(Hexagon hexA, Hexagon hexB)
    {
        KeyDouble key = hexA & hexB; //?????
        if (_crossLinks.ContainsKey(key) || (hexA.IsWater && hexB.IsWater))
            return false;

        HashSet<Crossroad> cross = new(hexA.Crossroads);
        cross.IntersectWith(hexB.Crossroads);
        if (cross.Count != 2)
            return false;

        Crossroad crossA, crossB;
        IEnumerator<Crossroad> enumerator = cross.GetEnumerator();
        crossA = GetCrossroad();
        crossB = GetCrossroad();

        _crossLinks.Add(key, new(key, crossA, crossB));

        return true;

        #region Local: GetCrossroad()
        //=================================
        Crossroad GetCrossroad()
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
        #endregion
    }

    private void SelectCrossroad(Crossroad cross)
    {
        EventSelectCrossroad?.Invoke(cross);
    }

#if UNITY_EDITOR
    public void Clear()
    {
        while (_thisTransform.childCount > 0)
            DestroyImmediate(_thisTransform.GetChild(0).gameObject);
    }
#endif
}
