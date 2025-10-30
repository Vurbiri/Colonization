using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract class AI<T> : IEquatable<Actor> where T : AStates
        {
            private static readonly Dictionary<Hexagon, Hexagon> s_links = new();
            private static readonly Queue<Hexagon> s_finds = new();

            protected static readonly RandomSequence s_haxIndexes = new(Crossroad.HEX_COUNT);
            protected static readonly WaitFrames s_waitBeforeSelecting = new(10);

            protected readonly Actor _actor;
            protected readonly T _action;

            protected AI(Actor actor)
            {
                _actor = actor;
                _action = (T)actor._states;
            }

            public bool Equals(Actor actor) => _actor == actor;
            public static bool Equals(AI<T> self, Actor actor) => self._actor == actor;

            protected static bool TryGetNextHexagon(Hexagon start, Hexagon end, out Hexagon next, out int pathLength)
            {
                int distance = start.Distance(end); 
                next = null; pathLength = -1;

                if (distance > (end.CanWarriorEnter ? 0 : 1) && Find(start, end, distance))
                {
                    next = s_links[start]; pathLength = 0;
                    while (s_links.TryGetValue(start, out end))
                    {
                        start = end;
                        pathLength++;
                    }
                }
                s_links.Clear(); 

                return pathLength >= 0;

                // ========== Local =================
                static bool Find(Hexagon start, Hexagon end, int distance)
                {
                    bool found = false;
                    int depth = distance + 1;
                    Hexagon current, near;

                    s_finds.Enqueue(end);
                    while (!found & s_finds.Count > 0)
                    {
                        current = s_finds.Dequeue();

                        for(int index = 0; !found & index < HEX.SIDES; index++)
                        {
                            near = current.Neighbors[index];
                            if (found = near == start)
                                s_links.Add(near, current);
                            if ((!found & near != end & near.CanWarriorEnter) && near.Distance(end) < depth && near.Distance(start) < depth && s_links.TryAdd(near, current))
                                s_finds.Enqueue(near);
                        }
                    }
                    s_finds.Clear();

                    return found;
                }
            }

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _actor.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine coroutine) => _actor.StopCoroutine(coroutine);
        }
    }
}
