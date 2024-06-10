using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CONST;

public class Roads : MonoBehaviour
{
    [Space]
    [SerializeField] private RoadLine _prefabRoadLine;

    private Transform _thisTransform;
    private Dictionary<KeyDouble, Road> _roads;
    private List<List<RoadLine>> _roadLineLists = new(Players.PLAYERS_MAX);
    private List<RoadLine> _currentRoadLines;

    public void Initialize(int circleMax)
    {
        //Debug.Log($"Count Roads calk: {HEX_SIDE * circleMax * (circleMax - 1)}");
        _roads = new(HEX_SIDE * circleMax * (circleMax - 1));
        _thisTransform = transform;

        for (int i = 0; i < Players.PLAYERS_MAX; i++)
            _roadLineLists.Add(new());
    }

    public void CreateRoad(Hexagon hexA, Hexagon hexB)
    {
        KeyDouble key = hexA & hexB; //?????
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

        _roads.Add(key, new(key, crossA, crossB));

        #region Local: GetCrossroad()
        //=================================
        Crossroad GetCrossroad()
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
        #endregion
    }

    public void BuildRoad(Road road, Player player)
    {
        Vector3 start = road.Start.Position, end = road.End.Position;
        _currentRoadLines = _roadLineLists[player.Id];

        road.Owner = player.Type;

        if (!AddRoadLine())
            NewRoadLine();

        StartCoroutine(CombiningRoadLine_Coroutine());

        #region Local: AddRoadLine(), NewRoadLine(), CombiningRoadLine_Coroutine()
        //=================================
        bool AddRoadLine()
        {
            foreach (var line in _currentRoadLines)
                if (line.TryAdd(start, end, road.End.IsFullOwned(road)))
                    return true;

            return false;
        }
        void NewRoadLine()
        {
            RoadLine roadLine;
            roadLine = Instantiate(_prefabRoadLine, transform);
            roadLine.Initialize(start, end, player.Color);
            _currentRoadLines.Add(roadLine);
        }
        IEnumerator CombiningRoadLine_Coroutine()
        {
            yield return null;
            RoadLine roadLine;
            for (int i = _currentRoadLines.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    roadLine = _currentRoadLines[i].Combining(_currentRoadLines[j]);
                    if (roadLine != null)
                    {
                        _currentRoadLines.Remove(roadLine);
                        Destroy(roadLine.gameObject);
                        StartCoroutine(CombiningRoadLine_Coroutine());
                        yield break;
                    }
                }
                yield return null;
            }
        }
        #endregion
    }

#if UNITY_EDITOR
    public void Clear()
    {
        while (_thisTransform.childCount > 0)
            DestroyImmediate(_thisTransform.GetChild(0).gameObject);
    }

    //public void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    foreach(var road in _roads.Values) 
    //    Gizmos.DrawLine(road.CrossA.Position, road.CrossB.Position);
    //}
#endif
}
