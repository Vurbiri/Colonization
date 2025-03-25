//Assets\Colonization\Scripts\Island\Road\Roads.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    public partial class Roads : IReactive<Roads>, IDisposable
    {
        #region Fields
        private readonly Id<PlayerId> _id;
        private readonly Gradient _gradient = new();
        private readonly RoadFactory _factory;
        private readonly List<Road> _roadsLists = new();
        private readonly RInt _count = new(0);
        private readonly Coroutines _coroutines;

        private readonly Subscriber<Roads> _subscriber = new();
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

        public void Restoration(Key[][] array, Crossroads crossroads)
        {
            for (int i = 0; i < array.Length; i++)
                CreateRoad(array[i], crossroads);

            #region Local: CreateRoad(...)
            //=================================
            void CreateRoad(Key[] keys, Crossroads crossroads)
            {
                int count = keys.Length;
                if (count < 2) return;

                Crossroad start = crossroads[keys[0]];
                for (int i = 1; i < count; i++)
                {
                    foreach (var link in start.Links)
                    {
                        if (link.Contains(keys[i]))
                        {
                            Build(link.SetStart(start));
                            start = link.End;
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
            _count.Increment();

            if (!AddRoadLine(link))
                _roadsLists.Add(_factory.Create(link.Start, link.End, _gradient));

            #region Local: AddRoadLine(..)
            //=================================
            bool AddRoadLine(CrossroadLink link)
            {
                for (int i = _roadsLists.Count - 1; i >= 0; i--)
                    if (_roadsLists[i].TryAdd(link.Start, link.End))
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
        public Unsubscriber Subscribe(Action<Roads> action, bool calling = false)
        {
            if (calling) action(this);
            return _subscriber.Add(action);
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

            _subscriber.Invoke(this);
        }

        public void Dispose()
        {
            _subscriber.Dispose();
        }
    }
}
