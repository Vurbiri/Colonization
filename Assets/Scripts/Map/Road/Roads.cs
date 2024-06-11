using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roads : MonoBehaviour
{
    [Space]
    [SerializeField] private Road _prefabRoad;

    private Transform _thisTransform;
    private List<List<Road>> _roadsLists = new(Players.PLAYERS_MAX);

    public void Initialize()
    {
        _thisTransform = transform;

        for (int i = 0; i < Players.PLAYERS_MAX; i++)
            _roadsLists.Add(new());
    }

    public void BuildRoad(CrossroadLink link, Player player)
    {
        List<Road> currentRoads = _roadsLists[player.Id];

        link.Owner = player.Type;

        if (!AddRoadLine())
            NewRoadLine();

        StartCoroutine(CombiningRoadLine_Coroutine());

        #region Local: AddRoadLine(), NewRoadLine(), CombiningRoadLine_Coroutine()
        //=================================
        bool AddRoadLine()
        {
            foreach (var line in currentRoads)
                if (line.TryAdd(link.Start, link.End))
                    return true;

            return false;
        }
        void NewRoadLine()
        {
            Road roadLine;
            roadLine = Instantiate(_prefabRoad, transform);
            roadLine.Initialize(link.Start, link.End, player);
            currentRoads.Add(roadLine);
        }
        IEnumerator CombiningRoadLine_Coroutine()
        {
            yield return null;
            Road roadLine;
            for (int i = currentRoads.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    roadLine = currentRoads[i].Union(currentRoads[j]);
                    if (roadLine != null)
                    {
                        currentRoads.Remove(roadLine);
                        Destroy(roadLine.gameObject);
                        StartCoroutine(CombiningRoadLine_Coroutine());
                        yield break;
                    }
                }
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
