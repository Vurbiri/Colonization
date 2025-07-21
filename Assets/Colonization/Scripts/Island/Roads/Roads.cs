using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class Roads : IReactive<Roads>
    {
        #region Fields
        private readonly int _id;
        private readonly RoadFactory _factory;
        private readonly List<Road> _roadsLists = new();
        private readonly RInt _count = new(0);
        private readonly Gradient _gradient;
        private readonly Coroutines _coroutines;
        private readonly Subscription<Roads> _eventChanged = new();
        #endregion

        public int Count => _count.Value;
        public IReactiveValue<int> CountReactive => _count;

        public Roads(Id<PlayerId> id, Color color, RoadFactory factory, Coroutines coroutines)
        {
            _id = id.Value;
            _factory = factory;
            _coroutines = coroutines;

            GradientAlphaKey[] alphas = { new(1.0f, 0.0f), new(1.0f, 1.0f) };
            GradientColorKey[] colors = { new(color, 0.0f), new(color, 1.0f) };
            _gradient = new() { colorKeys = colors, alphaKeys = alphas };
        }

        public ReturnSignal Build(CrossroadLink link, bool isSFX = false)
        {
            link.RoadBuilt(_id);
            _count.Increment();

            ReturnSignal returnSignal;

            for (int i = _roadsLists.Count - 1; i >= 0; i--)
                if (returnSignal = _roadsLists[i].TryAdd(link.Start, link.End, isSFX))
                    return returnSignal;

            Road road = _factory.Create(_gradient, _roadsLists.Count);
            returnSignal = road.Create(link.Start, link.End, isSFX);
            _roadsLists.Add(road);

            return returnSignal;
        }

        public ReturnSignal BuildAndUnion(CrossroadLink link)
        {
            ReturnSignal returnSignal = Build(link, true);
            _coroutines.StartCoroutine(TryUnion_Cn(returnSignal.signal));
            return returnSignal;
        }

        public int DeadEndsCount()
        {
            int deadEndsCount = 0;
            for (int i = _roadsLists.Count - 1; i >= 0; i--)
                deadEndsCount += _roadsLists[i].DeadEndsCount(_id);
            return deadEndsCount;
        }
        public bool RemoveDeadEnds()
        {
            Road line; int removeCount = 0;
            for (int i = _roadsLists.Count - 1; i >= 0; i--)
            {
                line = _roadsLists[i];
                removeCount += line.RemoveDeadEnds(_id);

                if (line.Count < 2)
                    _roadsLists.RemoveAt(i);
            }

            if (removeCount > 0)
            {
                _eventChanged.Invoke(this);
                _count.Remove(removeCount);
                return true;
            }
            return false;
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

            Road currentLine, removingLine;
            for (int i = _roadsLists.Count - 1; i > 0; i--)
            {
                currentLine = _roadsLists[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    removingLine = currentLine.Union(_roadsLists[j]);
                    if (removingLine != null)
                    {
                        _roadsLists.Remove(removingLine);
                        removingLine.Disable();
                        _coroutines.StartCoroutine(TryUnion_Cn(null));
                        yield break;
                    }
                }
                currentLine.SortingOrder = i;
            }

            _eventChanged.Invoke(this);
        }
    }
}
