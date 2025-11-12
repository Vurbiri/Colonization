using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI : IEquatable<Actor>, IDisposable
        {
            private static readonly Dictionary<Hexagon, Hexagon> s_links = new();
            private static readonly Queue<Hexagon> s_finds = new();

            protected static readonly RandomSequence s_crossroadHex = new(Crossroad.HEX_COUNT);
            protected static readonly RandomSequence s_hexagonIndexes = new(HEX.SIDES);
            protected static readonly WaitFrames s_waitBeforeSelecting = new(10);

            private readonly Status _status;
            private readonly Actor _actor;
            private readonly Goals _goals;
            private readonly ActorAISettings _aISettings;
            private readonly State _goalSetting;
            private State _current;

            [Impl(256)]
            protected AI(Actor actor, Goals goals, ActorAISettings aISettings)
            {
                _status = new();
                _actor = actor;
                _goals = goals;
                _aISettings = aISettings;
                _current = _goalSetting = new GoalSetting(this, GetStates());
            }

            public IEnumerator Execution_Cn()
            {
                int key;
                do
                {
#if TEST_AI
                    Log.Info($"[{ActorTypeId.GetName(_actor)}AI_{_actor.Index}] {_actor.Owner} state [{_current}]");
#endif
                    _status.Update(_actor);
                    yield return StartCoroutine(_current.Execution_Cn(Out<bool>.Get(out key)));
                    _status.Clear();
                }
                while (Out<bool>.Result(key));
            }

            public void Dispose() => _current.Dispose();

            protected abstract State[] GetStates();

            protected static void StatesSort(State[] states, ReadOnlyArray<int> priority)
            {
                for(int i = states.Length - 1, j; i > 0;)
                {
                    j = priority.IndexOf(states[i].Id);
                    if (i != j)
                        (states[i], states[j]) = (states[j], states[i]);
                    else
                        --i;
                }
            }

            #region ================= Pathfind ================== 
            protected static bool TryGetDistance(Actor actor, Hexagon end, int oldLength, out int pathLength, bool isEnterToGate = false)
            {
                var start = actor._currentHex;
                int distance = start.Distance(end);
                pathLength = 0;

                if (distance > (end.CanActorEnter(isEnterToGate) ? 0 : 1) && Pathfind(actor, end, distance, isEnterToGate))
                {
                    while (s_links.TryGetValue(start, out end))
                    {
                        start = end;
                        pathLength++;
                    }
                }
                s_links.Clear();

                return pathLength > 0 && (pathLength < oldLength || (pathLength == oldLength && Chance.Rolling()));
            }

            protected static bool TryGetNextHexagon(Actor actor, Hexagon end, out Hexagon next, bool isEnterToGate = false)
            {
                next = null;

                int distance = actor._currentHex.Distance(end);
                if (distance > (end.CanActorEnter(isEnterToGate) ? 0 : 1) && Pathfind(actor, end, distance, isEnterToGate))
                    next = s_links[actor._currentHex];
                s_links.Clear();

                return next != null;
            }

            private static bool Pathfind(Actor actor, Hexagon end, int distance, bool isEnterToGate)
            {
                bool found = false;

                s_finds.Enqueue(end);
                while (s_finds.Count > 0 && !(found = Find(actor._currentHex, end, distance, isEnterToGate))) ;
                s_finds.Clear();

                return found;

                // ========== Local ============
                static bool Find(Hexagon start, Hexagon end, int depth, bool isEnterToGate)
                {
                    Hexagon near, current = s_finds.Dequeue(); ;
                    foreach (int index in s_hexagonIndexes)
                    {
                        near = current.Neighbors[index];
                        if (near == start)
                        {
                            s_links.Add(near, current);
                            return true;
                        }

                        if (near != end && near.CanActorEnter(isEnterToGate) && (near.Distance(end) <= depth && near.Distance(start) <= depth) && s_links.TryAdd(near, current))
                            s_finds.Enqueue(near);
                    }
                    return false;
                }
            }
            #endregion

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _actor.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine coroutine) => _actor.StopCoroutine(coroutine);

            public bool Equals(Actor actor) => _actor == actor;
        }

    }
}
