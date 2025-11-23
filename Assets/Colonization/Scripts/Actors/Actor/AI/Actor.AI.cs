using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI : IDisposable
        {
            private static readonly Dictionary<Hexagon, Hexagon> s_links = new();
            private static readonly Queue<Hexagon> s_finds = new();

            protected static readonly RandomSequence s_crossroadHex = new(Crossroad.HEX_COUNT);
            protected static readonly RandomSequence s_hexagonIndexes = new(HEX.SIDES);

            public abstract IEnumerator Execution_Cn();
            public abstract void Dispose();

            #region ================= Pathfind ================== 
            protected static bool TryGetDistance(Actor actor, Hexagon end, int maxLength, out int pathLength)
            {
                pathLength = GetDistance(actor, end);
                return pathLength > 0 && (pathLength < maxLength || (pathLength == maxLength && Chance.Rolling()));
            }

            protected static int GetDistance(Actor actor, Hexagon end)
            {
                bool isEnterToGate = actor._owner == PlayerId.Satan && GameContainer.Players.Satan.CanEnterToGate;
                var start = actor._currentHex;
                int distance = start.Distance(end);

                if (distance == (end.CanActorEnter(isEnterToGate) ? 0 : 1))
                    return 0;

                int pathLength = -1;
                if (Pathfind(actor, end, distance, isEnterToGate))
                {
                    pathLength = 0;
                    while (s_links.TryGetValue(start, out end))
                    {
                        start = end;
                        pathLength++;
                    }
                }
                s_links.Clear();

                return pathLength;
            }

            protected static bool TryGetNextHexagon(Actor actor, Hexagon end, out Hexagon next)
            {
                bool isEnterToGate = actor._owner == PlayerId.Satan && GameContainer.Players.Satan.CanEnterToGate;
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
        }
    }
}
