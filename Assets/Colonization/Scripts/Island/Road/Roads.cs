using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Roads : MonoBehaviour
    {
        [SerializeField] private Road _prefabRoad;
        [SerializeField] private Currencies _cost;

        private Transform _thisTransform;
        private PlayerType _type;
        private Color _color;
        private readonly List<Road> _roadsLists = new();
        private int _count = 0;

        private const string NAME = "Roads_";

        public Currencies Cost => _cost;
        public int Count => _count;

        public Roads Initialize(PlayerType type, Color color)
        {
            _thisTransform = transform;
            _type = type;
            _color = color;
            name = NAME + type;

            return this;
        }

        public void Build(CrossroadLink link)
        {
            link.RoadBuilt(_type);
            _count++;

            if (!AddRoadLine(link))
                NewRoadLine(link);

            #region Local: AddRoadLine(), NewRoadLine()
            //=================================
            bool AddRoadLine(CrossroadLink link)
            {
                foreach (var line in _roadsLists)
                    if (line.TryAdd(link.Start, link.End))
                        return true;

                return false;
            }
            //=================================
            void NewRoadLine(CrossroadLink link)
            {
                Road roadLine;
                roadLine = Instantiate(_prefabRoad, _thisTransform);
                roadLine.Create(link.Start, link.End, _type, _color);
                _roadsLists.Add(roadLine);
            }
            #endregion
        }

        public void BuildAndUnion(CrossroadLink link)
        {
            Build(link);
            StartCoroutine(TryUnion_Coroutine());
        }

        public void TryUnion() => StartCoroutine(TryUnion_Coroutine());

        public int[][][] GetCrossroadsKey()
        {
            int count = _roadsLists.Count;
            int[][][] keys = new int[count][][];

            for (int i = 0; i < count; i++)
                keys[i] = _roadsLists[i].GetCrossroadsKey();

            return keys;
        }

        public void SetRoadsEndings()
        {
            foreach (var road in _roadsLists)
                road.SetGradient();
        }

        private IEnumerator TryUnion_Coroutine()
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
                        StartCoroutine(TryUnion_Coroutine());
                        yield break;
                    }
                }
            }
        }
    }
}
