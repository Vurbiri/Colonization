using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Actor;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI : AI<WarriorsAISettings, WarriorId, WarriorAIStateId>
    {
        private static string s_msg;

        static WarriorAI()
        {
            IdArray<WarriorAIStateId, System.Func<AI<WarriorsAISettings, WarriorId, WarriorAIStateId>, State>> factories = new();

            factories[WarriorAIStateId.Escape]         = GetEscape;
            factories[WarriorAIStateId.EscapeSupport]  = GetEscapeSupport;
            factories[WarriorAIStateId.BlockInCombat]  = GetBlockInCombat;
            factories[WarriorAIStateId.Combat]         = GetCombat;
            factories[WarriorAIStateId.Support]        = GetSupport;
            factories[WarriorAIStateId.MoveToHelp]     = GetMoveToHelp;
            factories[WarriorAIStateId.Defense]        = GetDefense;
            factories[WarriorAIStateId.MoveToUnsiege]  = GetMoveToUnsiege;
            factories[WarriorAIStateId.MoveToAttack]   = GetMoveToAttack;
            factories[WarriorAIStateId.MoveToRaid]     = GetMoveToRaid;
            factories[WarriorAIStateId.MoveToHome]     = GetMoveToHome;
            factories[WarriorAIStateId.FindResources]  = GetFindResources;

            SetFactories(factories);

            s_msg = "[WarriorAI] Initialized";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WarriorAI(Actor actor, Goals goals) : base(actor, goals) { }

        public static void Start()
        {
            if (s_msg != null)
            {
                Log.Info(s_msg); 
                s_msg = null;
            }
        }

        private static State GetEscape(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent)        => new Escape(parent);
        private static State GetEscapeSupport(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) => new EscapeSupport(parent);
        private static State GetBlockInCombat(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) => new BlockInCombat(parent);
        private static State GetSupport(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent)       => new Support(parent);
        private static State GetMoveToUnsiege(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) => new MoveToUnsiege(parent);
        private static State GetMoveToHome(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent)    => new MoveToHome(parent);
        private static State GetFindResources(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) => new FindResources(parent);

        public static bool TrySetSpawn(Human human, List<Hexagon> output)
        {
            output.Clear();
            var colonies = human.Colonies;
            var ports = human.Ports;
                                
            Crossroad crossroad; Hexagon hexagon;

            for (int i = 0; i < colonies.Count; ++i)
            {
                crossroad = colonies[i];
                if (crossroad.IsEnemyNear(human.Id) && TryGetNearFreeHexagon(GetNearPort(crossroad.Key, ports), crossroad, out hexagon))
                    output.Add(hexagon);
            }

            if (output.Count == 0)
            {
                for (int i = 0; i < colonies.Count; ++i)
                {
                    crossroad = colonies[i];
                    if (!crossroad.IsOwnerNear(human.Id) && TryGetNearFreeHexagon(GetNearPort(crossroad.Key, ports), crossroad, out hexagon))
                        output.Add(hexagon);
                }

                if (output.Count == 0)
                {
                    for (int i = 0; i < ports.Count; ++i)
                        SetFreeHexagons(ports[i], output);
                }
            }

            return output.Count > 0;

            #region Local: GetNearPort(..), TryGetNearFreeHexagon(..), SetFreeHexagons(..)
            // ====================================
            static Crossroad GetNearPort(Key colony, ReadOnlyReactiveList<Crossroad> ports)
            {
                var result = ports[0];
                int distance = CROSS.Distance(colony, result.Key);

                for (int i = 1, temp; i < ports.Count; ++i)
                {
                    temp = CROSS.Distance(colony, ports[i].Key);
                    if (temp < distance)
                    {
                        distance = temp;
                        result = ports[i];
                    }
                }
                return result;
            }
            // ====================================
            static bool TryGetNearFreeHexagon(Crossroad start, Crossroad target, out Hexagon output)
            {
                Hexagon current; output = null;

                for (int i = 0, temp, distance = int.MaxValue; i < Crossroad.HEX_COUNT; ++i)
                {
                    current = start.Hexagons[i];
                    if (current.CanWarriorEnter)
                    {
                        for (int t = 0; t < Crossroad.HEX_COUNT; t++)
                        {
                            temp = current.Distance(target.Hexagons[t]);
                            if (temp < distance)
                            {
                                distance = temp;
                                output = current;
                            }
                        }
                    }
                }

                return output != null;
            }
            // ====================================
            static void SetFreeHexagons(Crossroad crossroad, List<Hexagon> output)
            {
                Hexagon hexagon;
                for (int i = 0; i < Crossroad.HEX_COUNT; ++i)
                {
                    hexagon = crossroad.Hexagons[i];
                    if (hexagon.CanWarriorEnter)
                        output.Add(hexagon);
                }
            }
            // ====================================
            #endregion
        }
    }
}
