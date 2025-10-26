using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Warrior
    {
        public abstract class AI : AI<WarriorStates>
        {
            [Impl(256)] protected AI(Actor actor) : base(actor) { }


            public static bool TrySetSpawn(List<Hexagon> hexagons, Human human)
            {
                hexagons.Clear();
                var colonies = human.Colonies;
                var ports = human.Ports;
                                
                Crossroad crossroad; Hexagon hexagon = null;

                for (int i = 0; i < colonies.Count; i++)
                {
                    crossroad = colonies[i];
                    if (crossroad.IsEnemyNear(human.Id) && TryGetNearFreeHexagon(GetNearPort(crossroad.Key, ports), crossroad, ref hexagon))
                        hexagons.Add(hexagon);
                }

                if (hexagons.Count == 0)
                {
                    for (int i = 0; i < colonies.Count; i++)
                    {
                        crossroad = colonies[i];
                        if (crossroad.IsEmptyNear(human.Id) && TryGetNearFreeHexagon(GetNearPort(crossroad.Key, ports), crossroad, ref hexagon))
                            hexagons.Add(hexagon);
                    }

                    if (hexagons.Count == 0)
                    {
                        for (int i = 0; i < ports.Count; i++)
                            SetFreeHexagons(ports[i].Hexagons, hexagons);
                    }
                }

                return hexagons.Count > 0;

                // ============ Local ==============
                static Crossroad GetNearPort(Key colony, ReadOnlyReactiveList<Crossroad> ports)
                {
                    int index = 0;
                    for (int i = 0, temp, distance = int.MaxValue; i < ports.Count; i++)
                    {
                        temp = CROSS.Distance(colony, ports[i].Key);
                        if (temp < distance)
                        {
                            distance = temp;
                            index = i;
                        }
                    }
                    return ports[index];
                }
            }

            protected static bool TryGetNearFreeHexagon(Crossroad start, Crossroad target, ref Hexagon output)
            {
                var starts = start.Hexagons;
                int index = -1; Key current; 

                for (int i = 0, temp, distance = int.MaxValue; i < Crossroad.HEX_COUNT; i++)
                {
                    if (starts[i].CanWarriorEnter)
                    {
                        current = starts[i].Key;
                        for (int t = 0; t < Crossroad.HEX_COUNT; t++)
                        {
                            temp = HEX.Distance(current, target.Hexagons[t].Key);
                            if (temp < distance)
                            {
                                distance = temp;
                                index = i;
                            }
                        }
                    }
                }

                if(index > 0)
                    output = starts[index];

                return index > 0;
            }

            protected static void SetFreeHexagons(ReadOnlyArray<Hexagon> input, List<Hexagon> output)
            {
                Hexagon hexagon;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                {
                    hexagon = input[i];
                    if (hexagon.CanWarriorEnter)
                        output.Add(hexagon);
                }
            }
        }
    }
}
