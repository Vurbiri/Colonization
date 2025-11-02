using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class GoalSetting : AIState
        {
            private const int COUNT = 4;
            private readonly List<AIState> _states = new();

            public GoalSetting(WarriorAI parent) : base(parent)
            {
                _states.Add(new Combat(parent));
                _states.Add(new MoveToSieged(parent));
                _states.Add(new MoveToColony(parent));

                _states.Add(new Defense(parent));
            }

            public override bool TryEnter() => true;
            protected override void OnExit() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                for (int i = 0; i < COUNT; i++)
                {
                    if (TryEnter(_states[i]))
                    {
                        isContinue.Set(true);
                        yield break;
                    }
                    yield return null;
                }

                isContinue.Set(false);
            }
        }
    }
}
