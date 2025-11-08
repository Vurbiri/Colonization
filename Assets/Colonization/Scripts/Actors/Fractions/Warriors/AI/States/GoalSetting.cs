using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class GoalSetting : AIState
        {
            private readonly AIState[] _states;

            public GoalSetting(WarriorAI parent, Combat combat, Support support) : base(parent)
            {
                _states = new AIState[]
                {
                    combat, 
                    support,
                    new Defense(parent),

                    new MoveToUnsiege(parent),
                    new MoveToHelp(parent),
                    new MoveToRaid(parent),
                    new MoveToHome(parent),
                };
            }

            public override bool TryEnter() => true;
            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                isContinue.Set(false);

                for (int i = 0; !isContinue & i < _states.Length; i++)
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
