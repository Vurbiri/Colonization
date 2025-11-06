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
        public partial class AI<TAction, TStatus>
        {
            protected abstract class State<T> : State where T : AI<TAction, TStatus>
            {
                protected readonly T _parent;

                #region Parent Properties
                protected Actor Actor { [Impl(256)] get => _parent._actor; }
                protected Goals Goals { [Impl(256)] get => _parent._goals; }
                protected TStatus Status { [Impl(256)] get => _parent._status; }
                protected bool ActorInCombat { [Impl(256)] get => _parent._actor.IsInCombat(); }
                protected TAction Action { [Impl(256)] get => _parent._action; }
                #endregion

                [Impl(256)] protected State(T parent) => _parent = parent;

                [Impl(256)] protected bool TryEnter(State state)
                {
                    bool result = state.TryEnter();
                    if (result)
                        _parent._current = state;
                    return result;
                }

                [Impl(256)] protected void Exit()
                {
                    Dispose();
                    _parent._current = _parent._goalSetting;
                }
                [Impl(256)] protected void Exit(State newState)
                {
                    Dispose();
                    _parent._current = newState;
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

                protected bool TryGetEmptyColony(ReadOnlyReactiveList<Crossroad> colonies, HashSet<Key> targets, ref int distance, out Crossroad colony, out Hexagon target)
                {
                    bool result = false;
                    colony = null; target = null;
                    ReadOnlyArray<Hexagon> hexagons;
                    Hexagon hexTemp; Crossroad colonyTemp;

                    for (int i = 0; i < colonies.Count; i++)
                    {
                        colonyTemp = colonies[i];
                        if (!targets.Contains(colonyTemp) && (colonyTemp.ApproximateDistance(Actor.Hexagon) <= (distance + 1)) && colonyTemp.IsEmptyNear())
                        {
                            hexagons = colonyTemp.Hexagons;
                            foreach (int index in s_hexIndexes)
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
                    isExit |= Status.isInCombat;
                    if (!isExit && Action.CanUseMoveSkill())
                    {
                        isExit = !TryGetNextHexagon(Actor, target, out Hexagon next);
                        if (!isExit)
                        {
                            yield return Actor.StartCoroutine(Move_Cn(Action, next));
                            isExit = target.Distance(next) == distance;
                        }
                    }
                    isContinue.Set(isExit);
                    if (isExit) Exit();

                    // ======= Local =============
                    static IEnumerator Move_Cn(TAction action, Hexagon target)
                    {
                        yield return GameContainer.CameraController.ToPositionControlled(target.Position);

                        var wait = action.UseMoveSkill();

                        yield return s_waitBeforeSelecting;
                        action.Unselect(target);

                        yield return wait;
                    }
                }
            }
        }
        #endregion
    }
}
