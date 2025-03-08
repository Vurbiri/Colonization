//Assets\Colonization\Scripts\Island\Road\Roads.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    public class Roads : IReactive<int[][][]>, IDisposable
    {
        #region Fields
        private readonly Id<PlayerId> _id;
        private readonly Gradient _gradient = new();
        private readonly RoadFactory _factory;
        private readonly List<Road> _roadsLists = new();
        private readonly ReactiveValue<int> _count = new(0);
        private readonly Coroutines _coroutines;

        private Subscriber<int[][][]> _subscriber;
        #endregion

        public int Count => _count.Value;
        public IReactive<int> CountReactive => _count;

        public Roads(Id<PlayerId> id, Color color, RoadFactory factory, Coroutines coroutines)
        {
            _id = id;
            _factory = factory;
            _coroutines = coroutines;

            var alphas = new GradientAlphaKey[] { new(1.0f, 0.0f), new(1.0f, 1.0f) };
            var colors = new GradientColorKey[] { new(color, 0.0f), new(color, 1.0f) };
            _gradient.SetKeys(colors, alphas);
        }

        public void Restoration(IReadOnlyList<IReadOnlyList<Key>> array, Crossroads crossroads)
        {
            foreach (var keys in array)
                CreateRoad(keys, crossroads);

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
            _count.Value++;

            if (!AddRoadLine(link))
                _roadsLists.Add(_factory.Create(link.Start, link.End, _gradient));

            #region Local: AddRoadLine(..)
            //=================================
            bool AddRoadLine(CrossroadLink link)
            {
                foreach (var line in _roadsLists)
                    if (line.TryAdd(link.Start, link.End))
                        return true;

                return false;
            }
            #endregion
        }

        public void BuildAndUnion(CrossroadLink link)
        {
            Build(link);
            _coroutines.Run(TryUnion_Cn());
        }

        #region Reactive
        public Unsubscriber Subscribe(Action<int[][][]> action, bool calling = false)
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
                        Object.Destroy(roadLine.gameObject);
                        _coroutines.Run(TryUnion_Cn());
                        yield break;
                    }
                }
            }

            _subscriber.Invoke(ToArray());
        }

        public void Dispose()
        {
            _subscriber.Dispose();
        }
    }
}
