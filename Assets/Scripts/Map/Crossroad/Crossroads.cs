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
    private readonly Vector3[] _positionsCross = new Vector3[HEX_SIDE];

    private readonly float[] COS_CROSS = { COS_30, COS_90, -COS_30, -COS_30, -COS_90, COS_30 };
    private readonly float[] SIN_CROSS = { SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90, -SIN_30 };

    public void Initialize(int circleMax)
    {
        //Debug.Log($"Count Crossroads calk: {HEX_SIDE * circleMax * circleMax}");
        _crossroads = new(HEX_SIDE * circleMax * circleMax);
        
        float radiusPoint = HEX_DIAMETER * 0.5f;
        _offset = new(radiusPoint * COS_30, radiusPoint * SIN_30);
        for (int i = 0; i < HEX_SIDE; i++)
            _positionsCross[i] = new Vector3(radiusPoint * COS_CROSS[i], 0, radiusPoint * SIN_CROSS[i]);

        _thisTransform = transform;
    }

    public void CreateCrossroad(Vector3 position, Hexagon hex, bool isCircleMax)
    {
        Crossroad cross;
        Key key;
        Vector3 positionCross;
        for (int i = 0; i < HEX_SIDE; i++)
        {
            positionCross = _positionsCross[i] + position;

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
