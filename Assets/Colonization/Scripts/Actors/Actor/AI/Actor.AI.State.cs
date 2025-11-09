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
                protected Goals Goals { [Impl(256)] get => _parent._goals; }
                protected Status Status { [Impl(256)] get => _parent._status; }
                protected TAction Action { [Impl(256)] get => _parent._action; }
                protected bool IsInCombat { [Impl(256)] get => _parent._status.near.force > 0; }
                protected bool IsEnemyComing { [Impl(256)] get => _parent._status.nearTwo.force > 0; }
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

                [Impl(256)] protected bool EscapeChance(int enemyForce) => Status.isMove && Chance.Rolling((enemyForce * 10) / Actor.CurrentForce - 11);

                protected bool TryEscape(int minDistance, out Hexagon hexagon)
                {
                    Hexagon temp; hexagon = null;
                    var hexagons = Actor._currentHex.Neighbors;
                    foreach (int index in s_hexagonIndexes)
                    {
                        temp = hexagons[index];
                        if (temp.CanActorEnter(Actor.IsDemon) && CheckHexagon(minDistance, temp))
                        {
                            hexagon = temp;
                            break;
                        }
                    }

                    return hexagon != null;

                    #region Local: CheckHexagon(..), CheckActors(..)
                    //======================================================
                    bool CheckHexagon(int minDistance, Key target)
                    {
                        for(int i = Status.enemies.Count - 1; i >= 0; i--)
                            if (!CheckActors(GameContainer.Actors[Status.enemies[i]], minDistance, target))
                                return false;
                        return true;
                    }
                    //======================================================
                    static bool CheckActors(ReadOnlyReactiveSet<Actor> enemies, int minDistance, Key target)
                    {
                        foreach (var enemy in enemies)
                            if (enemy._currentHex.Distance(target) < minDistance)
                                return false;
                        return true;
                    }
                    #endregion
                }

                protected bool TryGetNearActorsInCombat(ReadOnlyReactiveSet<Actor> friends, ref int distance, out Actor enemy, out Actor friend)
                {
                    bool result = false;
                    List<Actor> enemies = new(HEX.SIDES); 
                    Actor enemyTemp; enemy = null; friend = null;

                    foreach (var friendTemp in friends)
                    {
                        int force = friendTemp.GetEnemiesNearAndForce(enemies);

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
                    if (!isExit && Action.CanUsedMoveSkill())
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
