using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class MoveToHelp : MoveTo
            {
                private ActorCode _targetEnemy;

                [Impl(256)] public MoveToHelp(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                sealed public override bool TryEnter()
                {
                    _targetHexagon = null;

                    if (Status.isMove && Status.percentHP > s_settings.minHPHelp)
                    {
                        int distance = s_settings.maxDistanceHelp;

                        for (int i = 0; i < PlayerId.Count; ++i)
                        {
                            if (GameContainer.Diplomacy.IsGreatFriend(OwnerId, i) && TryGetNearActorsInCombat(GameContainer.Actors[i], ref distance, out Actor enemy, out Actor friend))
                            {
                                _targetEnemy = enemy.Code;
                                _targetHexagon = (Settings.support ? friend : enemy).Hexagon;
                            }
                        }
                    }
                    return _targetHexagon != null && Goals.Enemies.Add(_targetEnemy, new(Actor));
                }

                sealed public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    bool isExit = !(Settings.support ? _targetHexagon.IsGreatFriend(OwnerId) : _targetHexagon.IsEnemy(OwnerId));
                    return Move_Cn(isContinue, 1, isExit, true, false);
                }

                sealed public override void Dispose()
                {
                    if (_targetHexagon != null)
                    {
                        _targetHexagon = null;
                        Goals.Enemies.Remove(_targetEnemy, new(Actor.Code));
                    }
                }

                private bool TryGetNearActorsInCombat(ReadOnlyReactiveSet<Actor> friends, ref int distance, out Actor enemy, out Actor friend)
                {
                    bool result = false;
                    List<Actor> enemies = new(HEX.SIDES);
                    Actor enemyTemp; enemy = null; friend = null;

                    foreach (var friendTemp in friends)
                    {
                        int force = GetEnemiesNearAndForce(friendTemp, enemies);

                        for (int i = enemies.Count - 1; i >= 0; --i)
                        {
                            enemyTemp = enemies[i];
                            if (Goals.Enemies.CanAdd(enemyTemp, force) && TryGetDistance(Actor, enemyTemp.Hexagon, distance, out int newDistance))
                            {
                                distance = newDistance;
                                enemy = enemyTemp;
                                friend = friendTemp;
                                result = true;
                            }
                        }
                        enemies.Clear();
                    }
                    return result;

                    // =============== Local ======================
                    static int GetEnemiesNearAndForce(Actor friend, List<Actor> enemies)
                    {
                        int force = 0;
                        var neighbors = friend.Hexagon.Neighbors;
                        for (int i = 0; i < HEX.SIDES; ++i)
                        {
                            if (neighbors[i].TryGetEnemy(friend.Owner, out Actor enemy))
                            {
                                enemies.Add(enemy);
                                force += enemy.CurrentForce;
                            }
                        }
                        return force;
                    }
                }
            }
        }
    }
}
