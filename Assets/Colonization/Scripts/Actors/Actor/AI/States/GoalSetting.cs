using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class GoalSetting : State
            {
                private readonly State[] _states;

                public GoalSetting(AI<TSettings, TActorId, TStateId> parent, State[] states) : base(parent)
                {
                    _states = states;
                }

                public override bool TryEnter() => true;
                public override void Dispose() { }

                public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    isContinue.Set(false);

                    while(!Action.IsAvailable)
                        yield return null;

                    for (int i = 0; !isContinue & i < _states.Length; ++i)
                    {
                        yield return null;
                        isContinue.Set(TryEnter(_states[i]));
                    }

                    // ====== Local ==========
                    bool TryEnter(State state)
                    {
                        bool result = state.TryEnter();
                        if (result)
                            _parent._current = state;
                        return result;
                    }
                }
            }
        }
    }
}
