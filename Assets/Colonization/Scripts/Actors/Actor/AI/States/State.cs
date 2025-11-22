using System;
using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI<TSettings, TActorId, TStateId>
        {
            protected abstract class State
            {
                protected readonly AI<TSettings, TActorId, TStateId> _parent;

                #region Parent Properties
                protected Actor Actor { [Impl(256)] get => _parent._actor; }
                protected Hexagon Hexagon { [Impl(256)] get => _parent._actor._currentHex; }
                protected Id<PlayerId> OwnerId { [Impl(256)] get => _parent._actor._owner; }
                protected States Action { [Impl(256)] get => _parent._actor._states; }
                protected Goals Goals { [Impl(256)] get => _parent._goals; }
                protected Status Status { [Impl(256)] get => _parent._status; }
                protected ActorAISettings Settings { [Impl(256)] get => _parent._aISettings; }
                protected bool IsInCombat { [Impl(256)] get => _parent._status.nearEnemies.IsForce; }
                protected bool IsEnemyComing { [Impl(256)] get => _parent._status.nighEnemies.IsForce; }
                #endregion

                [Impl(256)] protected State(AI<TSettings, TActorId, TStateId> parent) => _parent = parent;

                public abstract bool TryEnter();
                public abstract void Dispose();
                public abstract IEnumerator Execution_Cn(Out<bool> isContinue);

                sealed public override string ToString() => GetType().Name;

                [Impl(256)] protected void Exit()
                {
                    Dispose();
                    _parent._current = _parent._goalSetting;
                }

                protected bool TryGetEmptyColony(ReadOnlyReactiveList<Crossroad> colonies, ref int distance, out Crossroad colony, out Hexagon target, Func<Crossroad, bool> canAdd)
                {
                    bool result = false;
                    colony = null; target = null;
                    ReadOnlyArray<Hexagon> hexagons;
                    Hexagon hexTemp; Crossroad colonyTemp;

                    for (int i = 0; i < colonies.Count; ++i)
                    {
                        colonyTemp = colonies[i];
                        if (canAdd(colonyTemp) && (colonyTemp.ApproximateDistance(Hexagon) <= (distance + 1)) && colonyTemp.IsEmptyNear())
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
                            yield return Actor.Move_Cn(next);
                            isExit = target.Distance(next) == distance;
                        }
                    }
                    isContinue.Set(isExit);
                    if (isExit) Exit();
                }
            }
        }
    }
}

