using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roads : MonoBehaviour
{
    [SerializeField] private Road _prefabRoad;
    [SerializeField] private Currencies _cost;

    public Currencies Cost => _cost;

    private Transform _thisTransform;
    private PlayerType _type;
    private Color _color;
    private readonly List<Road> _roadsLists = new();

    private const string NAME = "Roads_";

    public Roads Initialize(PlayerType type, Color color)
    {
        _thisTransform = transform;
        _type = type;
        _color = color;
        name = NAME + type;

        return this;
    }

    public void BuildRoad(CrossroadLink link)
    {
        link.RoadBuilt(_type);

        if (!AddRoadLine())
            NewRoadLine();

        StartCoroutine(CombiningRoadLine_Coroutine());

        #region Local: AddRoadLine(), NewRoadLine(), CombiningRoadLine_Coroutine()
        //=================================
        bool AddRoadLine()
        {
            foreach (var line in _roadsLists)
                if (line.TryAdd(link.Start, link.End))
                    return true;

            return false;
        }
        void NewRoadLine()
        {
            Road roadLine;
            roadLine = Instantiate(_prefabRoad, _thisTransform);
            roadLine.Initialize(link.Start, link.End, _type, _color);
            _roadsLists.Add(roadLine);
        }
        IEnumerator CombiningRoadLine_Coroutine()
        {
            yield return null;
            Road roadLine;
            for (int i = _roadsLists.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    roadLine = _roadsLists[i].Union(_roadsLists[j]);
                    if (roadLine != null)
                    {
                        _roadsLists.Remove(roadLine);
                        Destroy(roadLine.gameObject);
                        StartCoroutine(CombiningRoadLine_Coroutine());
                        yield break;
                    }
                }
            }
        }
        #endregion
    }
}
