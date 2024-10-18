using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class Roads : MonoBehaviour, IEnumerable<int[][]>
    {
        [SerializeField] private Road _prefabRoad;
        [SerializeField] private PricesScriptable _prices;

        #region private
        private Transform _thisTransform;
        private Id<PlayerId> _id;
        private ACurrencies _cost;
        private Color _color;
        private readonly List<Road> _roadsLists = new();
        private int _count = 0;

#if UNITY_EDITOR
        private const string NAME = "Roads_";
#endif
        #endregion

        public ACurrencies Cost => _cost;
        public int Count => _count;

        public Roads Init(Id<PlayerId> id, Color color)
        {
            _thisTransform = transform;
            _id = id;
            _color = color;
            _cost = _prices.Road;
            _prices = null;

            
#if UNITY_EDITOR
            name = NAME + PlayerId.Names[id.ToInt - PlayerId.Min];
#endif
            return this;
        }

        public Roads Restoration(int[][][] array, Crossroads crossroads)
        {
            foreach (var keys in array)
                CreateRoad(keys, crossroads);

            SetRoadsEndings();
            return this;

            #region Local: CreateRoad(...)
            //=================================
            void CreateRoad(int[][] keys, Crossroads crossroads)
            {
                int count = keys.Length;
                if (count < 2) return;

                Key key = new(keys[0]);
                Crossroad start = crossroads[key];
                for (int i = 1; i < count; i++)
                {
                    foreach (var link in start.Links)
                    {
                        if (link.Contains(key.SetValues(keys[i])))
                        {
                            link.SetStart(start);
                            start = link.End;
                            Build(link);
                            break;
                        }
                    }
                }
            }
            #endregion
        }

        public void Build(CrossroadLink link)
        {
            link.RoadBuilt(_id);
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
                roadLine.Create(link.Start, link.End, _id, _color);
                _roadsLists.Add(roadLine);
            }
            #endregion
        }

        public void BuildAndUnion(CrossroadLink link)
        {
            Build(link);
            StartCoroutine(TryUnion_Coroutine());
        }

        public int[][][] ToArray()
        {
            int count = _roadsLists.Count;
            int[][][] keys = new int[count][][];

            for (int i = 0; i < count; i++)
                keys[i] = _roadsLists[i].ToArray();

            return keys;
        }

        private void SetRoadsEndings()
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

        public IEnumerator<int[][]> GetEnumerator()
        {
            int count = _roadsLists.Count;
            for (int i = 0; i < count; i++)
                yield return _roadsLists[i].ToArray();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if(_prefabRoad == null)
                _prefabRoad = VurbiriEditor.Utility.FindAnyPrefab<Road>();

            if (_prices == null)
                _prices = VurbiriEditor.Utility.FindAnyScriptable<PricesScriptable>();
        }
#endif
    }
}
