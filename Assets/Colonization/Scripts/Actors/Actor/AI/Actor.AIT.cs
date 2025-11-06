using System;
using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI<TAction, TStatus> : AI, IDisposable where TAction : AStates where TStatus : AI.AStatus, new()
        {
            private readonly TAction _action;
            private readonly TStatus _status;
            protected State _current, _goalSetting;

            [Impl(256)]
            protected AI(Actor actor, Goals goals) : base(actor, goals)
            {
                _action = (TAction)actor._states;
                _status = new();
            }

            public IEnumerator Execution_Cn()
            {
                int key;
                do
                {
                    Log.Info($"[WarriorAI] {_actor.Owner} state [{_current}]");

                    _status.Update(_actor);
                    yield return StartCoroutine(_current.Execution_Cn(Out<bool>.Get(out key)));
                }
                while (Out<bool>.Result(key));
            }

            public void Dispose() => _current.Dispose();
        }
    }
}
