using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class WarriorAI : Actor.AI<Warrior.WarriorStates>
    {
        protected static readonly WarriorAISettings s_settings;

        static WarriorAI() => s_settings = SettingsFile.Load<WarriorAISettings>();

        private readonly GoalSetting _goalSetting;
        private AIState _current;
        private Status _status = new(false);

        protected Id<PlayerId> _playerId;

        [Impl(256)] protected WarriorAI(Actor actor) : base(actor) 
        {
            _playerId = actor.Owner;
            _current = _goalSetting = new(this);
        }

        public static WarriorAI Create(Actor actor) => actor.Id switch
        {
            WarriorId.Militia => new MilitiaAI(actor),
            WarriorId.Solder  => new SolderAI(actor),
            WarriorId.Wizard  => new WizardAI(actor),
            WarriorId.Warlock => new WarlockAI(actor),
            WarriorId.Knight  => new KnightAI(actor),
            _ => null
        };

        public IEnumerator Execution_Cn()
        {
            int key;
            do
            {
                Log.Info($"[WarriorAI] {_playerId} state [{_current}]");

                _status.Update(_actor);
                yield return StartCoroutine(_current.Execution_Cn(Out<bool>.Get(out key)));
            }
            while (Out<bool>.Result(key));
        }

        public abstract IEnumerator Combat_Cn();

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

        // ******* Nested *******
        private struct Status
        {
            public bool isInCombat;
            public readonly List<ActorCode> enemies;
            public bool isGuard;
            public int minGuard;

            public Status(bool dummy)
            {
                isInCombat = isGuard = false;
                minGuard = int.MaxValue;
                enemies = new(3);
            }

            public void Update(Actor actor)
            {
                var hex = actor.Hexagon;
                var near = hex.Neighbors;
                var crossroads = hex.Crossroads;

                enemies.Clear();
                for (int i = 0; i < HEX.SIDES; i++)
                    if (near[i].TryGetEnemy(actor.Owner, out Actor enemy))
                        enemies.Add(enemy);
                isInCombat = enemies.Count > 0;

                minGuard = int.MaxValue;
                for (int i = 0, count; i < HEX.VERTICES; i++)
                {
                    count = crossroads[i].GetGuardCount(actor.Owner);
                    if (count > 0)
                    {
                        isGuard = true;
                        minGuard = System.Math.Min(minGuard, count);
                    }
                }
            }
        }
    }
}
