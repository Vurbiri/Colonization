using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class Roads : MonoBehaviour
{
    [Space]
    [SerializeField] private RoadLine _prefabRoadGraphic;

    private Transform _thisTransform;
    private Dictionary<KeyDouble, Road> _roads;
    RoadLine _roadLine;

    public void Initialize(int circleMax)
    {
        //Debug.Log($"Count Roads calk: {HEX_SIDE * circleMax * (circleMax - 1)}");
        _roads = new(HEX_SIDE * circleMax * (circleMax - 1));
        _thisTransform = transform;
    }

    public void CreateRoad(Hexagon hexA, Hexagon hexB)
    {
        KeyDouble key = hexA & hexB;
        if (_roads.ContainsKey(key) || (hexA.IsWater && hexB.IsWater))
            return;

        HashSet<Crossroad> cross = new(hexA.Crossroads);
        cross.IntersectWith(hexB.Crossroads);
        if(cross.Count != 2)
            return;

        Crossroad crossA, crossB;
        IEnumerator<Crossroad> enumerator = cross.GetEnumerator();
        crossA = GetCrossroad();
        crossB = GetCrossroad();

        _roads.Add(key, new(hexA, hexB, crossA, crossB, BuildRoad));

        #region Local: GetCrossroad()
        //=================================
        Crossroad GetCrossroad()
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
        #endregion
    }


    private bool BuildRoad(Vector3 start, Vector3 end)
    {
        if (_roadLine == null)
        {
            _roadLine = Instantiate(_prefabRoadGraphic, transform);
            _roadLine.Initialize(start, end);
            return true;
        }

        return _roadLine.TryAdd(start, end);
    }

#if UNITY_EDITOR
    public void Clear()
    {
        while (_thisTransform.childCount > 0)
            DestroyImmediate(_thisTransform.GetChild(0).gameObject);
    }
#endif

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach(var road in _roads.Values) 
        Gizmos.DrawLine(road.CrossA.Position, road.CrossB.Position);
    }
#endif

}
