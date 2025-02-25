//Assets\Colonization\Scripts\Island\Road\Roads.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Roads : MonoBehaviour, IReactive<int[][][]>
    {
        [SerializeField] private Road _prefabRoad;

        #region private
        private Transform _thisTransform;
        private Id<PlayerId> _id;
        private Color _color;
        private readonly List<Road> _roadsLists = new();
        private int _count = 0;

        private Subscriber<int[][][]> _subscriber = new();
        #endregion

        public int Count => _count;

        public Roads Init(Id<PlayerId> id, Color color)
        {
            _thisTransform = transform;
            _id = id;
            _color = color;

            return this;
        }

        public Roads Restoration(IReadOnlyList<IReadOnlyList<Key>> array, Crossroads crossroads)
        {
            foreach (var keys in array)
                CreateRoad(keys, crossroads);

            SetRoadsEndings();
            return this;

            #region Local: CreateRoad(...)
            //=================================
            void CreateRoad(IReadOnlyList<Key> keys, Crossroads crossroads)
            {
                int count = keys.Count;
                if (count < 2) return;

                Crossroad start = crossroads[keys[0]];
                for (int i = 1; i < count; i++)
                {
                    foreach (var link in start.Links)
                    {
                        if (link.Contains(keys[i]))
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
            StartCoroutine(TryUnion_Cn());
        }

        #region Reactive
        public IUnsubscriber Subscribe(Action<int[][][]> action, bool calling = false)
        {
            if (calling)
                action(ToArray());

            return _subscriber.Add(action);
        }

        private int[][][] ToArray()
        {
            int count = _roadsLists.Count;
            int[][][] keys = new int[count][][];

            for (int i = 0; i < count; i++)
                keys[i] = _roadsLists[i].ToArray();

            return keys;
        }
        #endregion

        private void SetRoadsEndings()
        {
            foreach (var road in _roadsLists)
                road.SetGradient();
        }

        private IEnumerator TryUnion_Cn()
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
                        StartCoroutine(TryUnion_Cn());
                        yield break;
                    }
                }
            }

            _subscriber.Invoke(ToArray());
        }

       

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if(_prefabRoad == null)
                _prefabRoad = VurbiriEditor.Utility.FindAnyPrefab<Road>();
        }
#endif
    }
}
