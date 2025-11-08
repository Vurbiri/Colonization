using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI : IEquatable<Actor>
        {
            private static readonly Dictionary<Hexagon, Hexagon> s_links = new();
            private static readonly Queue<Hexagon> s_finds = new();

            protected static readonly RandomSequence s_crossroadHex = new(Crossroad.HEX_COUNT);
            protected static readonly RandomSequence s_hexagonIndexes = new(HEX.SIDES);
            protected static readonly WaitFrames s_waitBeforeSelecting = new(10);

            protected readonly Actor _actor;
            protected readonly Goals _goals;

            [Impl(256)]
            protected AI(Actor actor, Goals goals)
            {
                _actor = actor;
                _goals = goals;
            }

            #region ================= Pathfind ================== 
            protected static bool TryGetDistance(Actor actor, Hexagon end, int oldLength, out int pathLength)
            {
                var start = actor._currentHex;
                int distance = start.Distance(end);
                pathLength = 0;

                if (distance > (end.CanActorEnter(actor.IsDemon) ? 0 : 1) && Pathfind(actor, end, distance))
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

            protected static bool TryGetNextHexagon(Actor actor, Hexagon end, out Hexagon next)
            {
                next = null;

                int distance = actor._currentHex.Distance(end);
                if (distance > (end.CanActorEnter(actor.IsDemon) ? 0 : 1) && Pathfind(actor, end, distance))
                    next = s_links[actor._currentHex];
                s_links.Clear();

                return next != null;
            }

            private static bool Pathfind(Actor actor, Hexagon end, int distance)
            {
                bool found = false;

                s_finds.Enqueue(end);
                while (s_finds.Count > 0 && !(found = Find(actor._currentHex, end, distance, actor.IsDemon))) ;
                s_finds.Clear();

                return found;

                // ========== Local ============
                static bool Find(Hexagon start, Hexagon end, int depth, bool isDemon)
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

                        if (near != end && near.CanActorEnter(isDemon) && (near.Distance(end) <= depth && near.Distance(start) <= depth) && s_links.TryAdd(near, current))
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
