using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

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
        private readonly VAction<Roads> _eventChanged = new();
        #endregion

        public Road this[int index] { [Impl(256)] get => _roadsLists[index]; }
        public ReactiveValue<int> Count { [Impl(256)] get => _count; }

        public Roads(Id<PlayerId> id, RoadFactory factory)
        {
            _id = id.Value;
            _factory = factory;

            Color color = GameContainer.UI.PlayerColors[id];
            GradientAlphaKey[] alphas = { new(1.0f, 0.0f), new(1.0f, 1.0f) };
            GradientColorKey[] colors = { new(color, 0.0f), new(color, 1.0f) };
            _gradient = new() { colorKeys = colors, alphaKeys = alphas };
        }

        public ReturnSignal Build(CrossroadLink link, bool isSFX = false)
        {
            link.RoadBuilt(_id);
            _count.Increment();

            ReturnSignal returnSignal = false;

            for (int i = _roadsLists.Count - 1; i >= 0; i--)
                if (returnSignal = _roadsLists[i].TryAdd(link.Start, link.End, isSFX))
                    break;

            if (!returnSignal)
            {
                Road road = _factory.Create(_gradient, _roadsLists.Count);
                returnSignal = road.Create(link.Start, link.End, isSFX);
                _roadsLists.Add(road);
            }

            if(isSFX) _factory.RoadSFX.Build(link);

            return returnSignal;
        }

        public ReturnSignal BuildAndUnion(CrossroadLink link)
        {
            ReturnSignal returnSignal = Build(link, true);
            TryUnion_Cn(returnSignal.signal).Start();
            return returnSignal;
        }

        [Impl(256)]public bool ThereDeadEnds()
        {
            for (int i = _roadsLists.Count - 1; i >= 0; i--)
               if(_roadsLists[i].ThereDeadEnds(_id)) return true;
            
            return false;
        }

        public void SetDeadEnds(List<Crossroad> deadEnds)
        {
            for (int i = _roadsLists.Count - 1; i >= 0; i--)
                TryAddLine(deadEnds, _roadsLists[i]);

            #region Local
            [Impl(256)] void TryAddLine(List<Crossroad> deadEnds, Road line)
            {
                TryAdd(deadEnds, line.StartCrossroad);
                TryAdd(deadEnds, line.EndCrossroad);
            }
            [Impl(256)] void TryAdd(List<Crossroad> deadEnds, Crossroad crossroad)
            {
                if(crossroad.IsDeadEnd(_id)) deadEnds.Add(crossroad);
            }
            #endregion
        }

        public IEnumerator RemoveDeadEnds_Cn()
        {
            List<RemoveLink> removeLinks; RemoveLink removed;
            Road line; int removeCount = 0;
            for (int i = _roadsLists.Count - 1; i >= 0; i--)
            {
                line = _roadsLists[i];
                removeLinks = line.GetDeadEnds(_id);

                for (int j = removeLinks.Count - 1; j >= 0; j--)
                {
                    removeCount++;
                    removed = removeLinks[j];

                    yield return GameContainer.CameraController.ToPosition(removed.link.Position, true);

                    if (line.Remove(removed.isEnd)) 
                        _roadsLists.RemoveAt(i);

                    yield return _factory.RoadSFX.Remove_Cn(removed.link);
                }
            }

            if (removeCount > 0)
            {
                _eventChanged.Invoke(this);
                _count.Remove(removeCount);
            }
        }

        #region Reactive
        public Subscription Subscribe(Action<Roads> action, bool calling = false)
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
                        TryUnion_Cn(null).Start();
                        yield break;
                    }
                }
                currentLine.SortingOrder = i;
            }

            _eventChanged.Invoke(this);
        }
    }
}
