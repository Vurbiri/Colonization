using System.Collections;
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
            protected enum State { Defence, Attack, Combat }
            
            protected static readonly WarriorAISettings s_settings;

            static AI() => s_settings = SettingsFile.Load<WarriorAISettings>();

            protected State _state = State.Defence;
            protected Id<PlayerId> _playerId;

            [Impl(256)] protected AI(Actor actor) : base(actor) 
            {
                _playerId = actor.Owner;
            }

            public IEnumerator Execution_Cn()
            {
                yield return GameContainer.CameraController.ToPositionControlled(_actor.Position);

                if (_actor.IsInCombat())
                {
                    yield return StartCoroutine(Combat_Cn());
                    yield break;
                }

                if (_action.CanUseMoveSkill())
                {
                    var colonies = GameContainer.Players.Humans[_playerId].Colonies;
                    bool moving = FindSiegedEnemy(colonies, out Hexagon next);

                    if (!moving)
                    {
                        yield return null;
                        moving = FindEmptyColony(colonies, out next);
                    }

                    if(moving)
                    {
                        var wait = _action.UseMoveSkill();

                        yield return s_waitBeforeSelecting;
                        _actor.Unselect(next);

                        yield return wait;

                        if (_actor.IsInCombat())
                            yield return StartCoroutine(Combat_Cn());

                        yield break;
                    }
                }

                Key current = _actor.Hexagon.Key;

                if (IsEnemyComing(current))
                {
                    if(_action.CanUseSpecSkill())
                        yield return _action.UseSpecSkill();
                    yield break;
                }

                #region Local: FindSiegedEnemy(..), IsEnemyComing(..)
                // ====================================
                bool FindSiegedEnemy(ReadOnlyReactiveList<Crossroad> colonies, out Hexagon next)
                {
                    Hexagon current, start = _actor.Hexagon; next = null;
                    ReadOnlyArray<Hexagon> hexagons;
                    int distance = s_settings.maxDistanceSiege, temp;

                    for (int c = 0; c < colonies.Count; c++)
                    {
                        hexagons = colonies[c].Hexagons;
                        for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                        {
                            current = hexagons[i];
                            if (current.IsEnemy(_playerId) && TryGetNextHexagon(start, current, out current, out temp) && temp < distance)
                            {
                                distance = temp;
                                next = current;
                            }
                        }
                    }
                    return next != null;
                }
                // ====================================
                bool FindEmptyColony(ReadOnlyReactiveList<Crossroad> colonies, out Hexagon next)
                {
                    bool find = true; next = null;
                    Hexagon start = _actor.Hexagon;
                     for (int i = 0; find & i < HEX.VERTICES; i++)
                        if (start.Crossroads[i].GetGuardCount(_playerId) == 1)
                            find = false;

                    if (find)
                    {
                        Hexagon current;
                        ReadOnlyArray<Hexagon> hexagons;
                        int distance = s_settings.maxDistanceEmpty, temp;

                        for (int i = 0; i < colonies.Count; i++)
                        {
                            if (colonies[i].IsEmptyNear(_playerId))
                            {
                                hexagons = colonies[i].Hexagons;
                                foreach (int index in s_haxIndexes)
                                {
                                    if (TryGetNextHexagon(start, hexagons[index], out current, out temp) && temp < distance)
                                    {
                                        distance = temp;
                                        next = current;
                                    }
                                }
                            }
                        }
                    }
                    return next != null;
                }
                // ====================================
                bool IsEnemyComing(Key current)
                {
                    bool result = false; Hexagon hex;

                    for (int i = 0; !result & i < HEX.NEAR_TWO.Count; i++)
                        result = GameContainer.Hexagons.TryGet(current + HEX.NEAR_TWO[i], out hex) && hex.IsEnemy(_playerId);

                    return result;
                }
                // ====================================
                #endregion
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
        }
    }
}
