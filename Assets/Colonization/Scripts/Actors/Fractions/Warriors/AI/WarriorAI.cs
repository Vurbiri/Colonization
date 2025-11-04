using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI : Actor.AI<Warrior.WarriorStates>, IDisposable
    {
        protected static readonly WarriorAISettings s_settings;

        static WarriorAI() => s_settings = SettingsFile.Load<WarriorAISettings>();

        private readonly Goals _goals;
        private readonly Situation _situation;
        private readonly GoalSetting _goalSetting;
        private AIState _current;

        [Impl(256)] protected WarriorAI(Actor actor, Goals goals) : base(actor) 
        {
            _goals = goals;
            _situation = new();
            _current = _goalSetting = new(this);
        }

        public static WarriorAI Create(Actor actor, Goals goals) => actor.Id switch
        {
            WarriorId.Militia => new WarriorAI(actor, goals),
            WarriorId.Solder  => new WarriorAI(actor, goals),
            WarriorId.Wizard  => new WarriorAI(actor, goals),
            WarriorId.Warlock => new WarriorAI(actor, goals),
            WarriorId.Knight  => new WarriorAI(actor, goals),
            _ => null
        };

        public IEnumerator Execution_Cn()
        {
            int key;
            do
            {
                Log.Info($"[WarriorAI] {_actor.Owner} state [{_current}]");

                _situation.Update(_actor);
                yield return StartCoroutine(_current.Execution_Cn(Out<bool>.Get(out key)));
            }
            while (Out<bool>.Result(key));
        }

        public static bool TrySetSpawn(Human human, List<Hexagon> output)
        {
            output.Clear();
            var colonies = human.Colonies;
            var ports = human.Ports;
                                
            Crossroad crossroad; Hexagon hexagon;

            for (int i = 0; i < colonies.Count; i++)
            {
                crossroad = colonies[i];
                if (crossroad.IsEnemyNear(human.Id) && TryGetNearFreeHexagon(GetNearPort(crossroad.Key, ports), crossroad, out hexagon))
                    output.Add(hexagon);
            }

            if (output.Count == 0)
            {
                for (int i = 0; i < colonies.Count; i++)
                {
                    crossroad = colonies[i];
                    if (crossroad.IsEmptyNear(human.Id) && TryGetNearFreeHexagon(GetNearPort(crossroad.Key, ports), crossroad, out hexagon))
                        output.Add(hexagon);
                }

                if (output.Count == 0)
                {
                    for (int i = 0; i < ports.Count; i++)
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

                for (int i = 1, temp; i < ports.Count; i++)
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

                for (int i = 0, temp, distance = int.MaxValue; i < Crossroad.HEX_COUNT; i++)
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
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                {
                    hexagon = crossroad.Hexagons[i];
                    if (hexagon.CanWarriorEnter)
                        output.Add(hexagon);
                }
            }
            // ====================================
            #endregion
        }

        public void Dispose() => _current.Dispose();
    }
}
