using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        #region =========== AI.State ==================
        public partial class AI
        {
            protected abstract class State
            {
                public abstract bool TryEnter();
                public abstract void Dispose();
                public abstract IEnumerator Execution_Cn(Out<bool> isContinue);

                sealed public override string ToString() => GetType().Name;
            }
        }
        #endregion

        #region =========== AI.State<T>  ==================
        public partial class AI<TAction>
        {
            protected abstract class State<T> : State where T : AI<TAction>
            {
                protected readonly T _parent;

                #region Parent Properties
                protected Actor Actor { [Impl(256)] get => _parent._actor; }
                protected Id<PlayerId> OwnerId { [Impl(256)] get => _parent._actor._owner; }
                protected TAction Action { [Impl(256)] get => _parent._action; }
                protected Goals Goals { [Impl(256)] get => _parent._goals; }
                protected Status Status { [Impl(256)] get => _parent._status; }
                protected ActorAISettings Settings { [Impl(256)] get => _parent._aISettings; }
                protected bool Support { [Impl(256)] get => _parent._aISettings.support; }
                protected bool Raider { [Impl(256)] get => _parent._aISettings.raider; }
                protected bool IsInCombat { [Impl(256)] get => _parent._status.near.enemiesForce > 0; }
                protected bool IsEnemyComing { [Impl(256)] get => _parent._status.nearTwo.enemiesForce > 0; }
                #endregion

                [Impl(256)] protected State(T parent) => _parent = parent;

                [Impl(256)] protected void TryExitTo(State newState)
                {
                    if (newState.TryEnter())
                    {
                        Dispose();
                        _parent._current = newState;
                    }
                }
                [Impl(256)] protected void Exit()
                {
                    Dispose();
                    _parent._current = _parent._goalSetting;
                }

                protected IEnumerator Defense_Cn(bool isBuff, bool isBlock)
                {
                    if (isBuff && Settings.defenseBuff.CanUsed(Action, Actor))
                        yield return Settings.defenseBuff.Use(Action);
                    if (isBlock && Action.CanUsedSpecSkill() && Settings.specChance.Roll)
                        yield return Action.UseSpecSkill();
                }

                protected bool TryGetNearActorsInCombat(ReadOnlyReactiveSet<Actor> friends, ref int distance, out Actor enemy, out Actor friend)
                {
                    bool result = false;
                    List<Actor> enemies = new(HEX.SIDES); 
                    Actor enemyTemp; enemy = null; friend = null;

                    foreach (var friendTemp in friends)
                    {
                        int force = GetEnemiesNearAndForce(friendTemp, enemies);

                        for (int i = enemies.Count - 1; i >= 0; i--)
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
                    static int GetEnemiesNearAndForce(Actor friend, List< Actor> enemies)
                    {
                        int force = 0;
                        var neighbors = friend._currentHex.Neighbors;
                        for (int i = 0; i < HEX.SIDES; i++)
                        {
                            if (neighbors[i].TryGetEnemy(friend._owner, out Actor enemy))
                            {
                                enemies.Add(enemy);
                                force += enemy.CurrentForce;
                            }
                        }
                        return force;
                    }
                }

                protected bool TryGetEmptyColony(ReadOnlyReactiveList<Crossroad> colonies, ref int distance, out Crossroad colony, out Hexagon target, Func<Crossroad, bool> canAdd)
                {
                    bool result = false;
                    colony = null; target = null;
                    ReadOnlyArray<Hexagon> hexagons;
                    Hexagon hexTemp; Crossroad colonyTemp;

                    for (int i = 0; i < colonies.Count; i++)
                    {
                        colonyTemp = colonies[i];
                        if (canAdd(colonyTemp) && (colonyTemp.ApproximateDistance(Actor.Hexagon) <= (distance + 1)) && colonyTemp.IsEmptyNear())
                        {
                            hexagons = colonyTemp.Hexagons;
                            foreach (int index in s_crossroadHex)
                            {
                                hexTemp = hexagons[index];
                                if (TryGetDistance(Actor, hexTemp, distance, out int newDistance))
                                {
                                    distance = newDistance;
                                    colony = colonyTemp;
                                    target = hexTemp;
                                    result = true;
                                }
                            }
                        }
                    }

                    return result;
                }

                protected IEnumerator Move_Cn(Out<bool> isContinue, int distance, Hexagon target, bool isExit = false)
                {
                    isExit |= IsInCombat;
                    if (!isExit && Status.isMove)
                    {
                        isExit = !TryGetNextHexagon(Actor, target, out Hexagon next);
                        if (!isExit)
                        {
                            yield return Actor.StartCoroutine(Move_Cn(next));
                            isExit = target.Distance(next) == distance;
                        }
                    }
                    isContinue.Set(isExit);
                    if (isExit) Exit();
                }

                protected IEnumerator Move_Cn(Hexagon target)
                {
                    yield return GameContainer.CameraController.ToPositionControlled(target.Position);

                    var wait = Action.UseMoveSkill();

                    yield return s_waitBeforeSelecting;
                    Action.Unselect(target);

                    yield return wait;
                }
            }
        }
        #endregion
    }
}
