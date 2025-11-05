using System.Collections;
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
        public partial class AI<TAction, TSituation>
        {
            protected abstract class State<T> : State where T : AI<TAction, TSituation>
            {
                protected readonly T _parent;

                #region Parent Properties
                protected Actor Actor { [Impl(256)] get => _parent._actor; }
                protected Goals Goals { [Impl(256)] get => _parent._goals; }
                protected TSituation Situation { [Impl(256)] get => _parent._situation; }
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

                protected IEnumerator Move_Cn(Out<bool> isContinue, int distance, Hexagon target, bool isExit = false)
                {
                    isExit |= Situation.isInCombat;
                    if (!isExit && Action.CanUseMoveSkill())
                    {
                        isExit = !TryGetNextHexagon(Actor._currentHex, target, out Hexagon next);
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
