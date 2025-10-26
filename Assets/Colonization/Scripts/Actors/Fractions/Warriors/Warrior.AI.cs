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



            public static List<Hexagon> GetHexagonsSpawn(Human human)
            {
                List<Hexagon> hexagons = new();
                var colonies = human.Colonies;
                var ports = human.Ports;
                                
                Crossroad crossroad;

                for (int i = 0; i < colonies.Count; i++)
                {
                    crossroad = colonies[i];
                    if (crossroad.IsEnemyNear(human.Id))
                        SetFreeHexagons(GetNearestPort(crossroad.Key, ports).Hexagons, hexagons);
                }

                if (hexagons.Count == 0)
                {
                    for (int i = 0; i < colonies.Count; i++)
                    {
                        crossroad = colonies[i];
                        if (crossroad.IsEmptyNear(human.Id))
                            SetFreeHexagons(GetNearestPort(crossroad.Key, ports).Hexagons, hexagons);
                    }

                    if (hexagons.Count == 0)
                    {
                        for (int i = 0; i < ports.Count; i++)
                            SetFreeHexagons(ports[i].Hexagons, hexagons);
                    }
                }

                return hexagons;

                // ============ Local ==============
                static Crossroad GetNearestPort(Key colony, ReadOnlyReactiveList<Crossroad> ports)
                {
                    int distance = int.MaxValue, index = 0;
                    for (int i = 0, temp; i < ports.Count; i++)
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
