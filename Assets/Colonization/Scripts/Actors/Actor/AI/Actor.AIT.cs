using System;
using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI<TAction, TSituation> : AI, IDisposable where TAction : AStates where TSituation : AI.ASituation, new()
        {
            private readonly TAction _action;
            private readonly TSituation _situation;
            protected State _current, _goalSetting;

            [Impl(256)]
            protected AI(Actor actor, Goals goals) : base(actor, goals)
            {
                _action = (TAction)actor._states;
                _situation = new();
            }

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

            public void Dispose() => _current.Dispose();

            protected IEnumerator Move_Cn(Out<bool> isContinue, int distance, Hexagon target, bool isExit)
            {
                if (!isExit && _action.CanUseMoveSkill())
                {
                    isExit = !TryGetNextHexagon(_actor._currentHex, target, out Hexagon next);
                    if (!isExit)
                    {
                        yield return StartCoroutine(Move_Cn(next));
                        isExit = target.Distance(next) == distance;
                    }
                }
                isContinue.Set(isExit);

                // ======= Local =============
                IEnumerator Move_Cn(Hexagon target)
                {
                    yield return GameContainer.CameraController.ToPositionControlled(target.Position);

                    var wait = _action.UseMoveSkill();

                    yield return s_waitBeforeSelecting;
                    _action.Unselect(target);

                    yield return wait;
                }
            }
        }
    }
}
