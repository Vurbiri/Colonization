using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    public partial class Roads : IReactive<Roads>
    {
        #region Fields
        private readonly Id<PlayerId> _id;
        private readonly RoadFactory _factory;
        private readonly List<Road> _roadsLists = new();
        private readonly RInt _count = new(0);
        private readonly Coroutines _coroutines;
        private readonly GradientAlphaKey[] _alphas  = { new (1.0f, 0.0f), new (1.0f, 1.0f) };
        private readonly GradientColorKey[] _colors = new GradientColorKey[2];

        private readonly Subscription<Roads> _eventChanged = new();
        #endregion

        public int Count => _count.Value;
        public IReactiveValue<int> CountReactive => _count;

        public Roads(Id<PlayerId> id, Color color, RoadFactory factory, Coroutines coroutines)
        {
            _id = id;
            _factory = factory;
            _coroutines = coroutines;
            _colors[0] = new(color, 0.0f); _colors[1] = new(color, 1.0f);
        }

        public ReturnSignal Build(CrossroadLink link, bool isSFX = false)
        {
            link.RoadBuilt(_id);
            _count.Increment();

            ReturnSignal returnSignal;

            for (int i = _roadsLists.Count - 1; i >= 0; i--)
                if (returnSignal = _roadsLists[i].TryAdd(link.Start, link.End, isSFX))
                    return returnSignal;

            Road road = _factory.Create(new() { colorKeys = _colors, alphaKeys = _alphas });
            returnSignal = road.CreateFirst(link.Start, link.End, isSFX);
            _roadsLists.Add(road);

            return returnSignal;
        }

        public ReturnSignal BuildAndUnion(CrossroadLink link)
        {
            ReturnSignal returnSignal = Build(link, true);
            _coroutines.Run(TryUnion_Cn(returnSignal));
            return returnSignal;
        }

        #region Reactive
        public Unsubscription Subscribe(Action<Roads> action, bool calling = false)
        {
            if (calling) action(this);
            return _eventChanged.Add(action);
        }
        #endregion

        private IEnumerator TryUnion_Cn(WaitSignal signal)
        {
            yield return signal;

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
                        _coroutines.Run(TryUnion_Cn(null));
                        yield break;
                    }
                }
            }

            _eventChanged.Invoke(this);
        }
    }
}
