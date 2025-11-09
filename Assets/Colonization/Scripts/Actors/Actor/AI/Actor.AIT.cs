using System;
using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI<TAction> : AI, IDisposable where TAction : AStates
        {
            private readonly TAction _action;
            private readonly Status _status;
            protected State _current, _goalSetting;
            
            [Impl(256)]
            protected AI(Actor actor, Goals goals) : base(actor, goals)
            {
                _action = (TAction)actor._states;
                _status = new(actor._owner);
            }

            public IEnumerator Execution_Cn()
            {
                int key;
                do
                {
#if TEST_AI
                    Log.Info($"[{ActorTypeId.GetName(_actor)}AI_{_actor.Index}] {_actor.Owner} state [{_current}]");
#endif
                    _status.Update(_actor);
                    yield return StartCoroutine(_current.Execution_Cn(Out<bool>.Get(out key)));
                    _status.Clear();
                }
                while (Out<bool>.Result(key));
            }

            public void Dispose() => _current.Dispose();
        }
    }
}
