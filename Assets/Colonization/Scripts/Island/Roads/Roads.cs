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
        private readonly Id<PlayerId> _id;
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
            _id = id;
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
            _coroutines.Run(TryUnion_Cn(returnSignal.signal));
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
                        removingLine.Destroy();
                        _coroutines.Run(TryUnion_Cn(null));
                        yield break;
                    }
                }
                currentLine.SortingOrder = i;
            }

            _eventChanged.Invoke(this);
        }
    }
}
