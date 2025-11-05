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

            public GoalSetting(WarriorAI parent, Combat combat) : base(parent)
            {
                _states.Add(combat);
                _states.Add(new MoveToUnsiege(parent));
                _states.Add(new MoveToColony(parent));

                _states.Add(new Defense(parent));
            }

            public override bool TryEnter() => true;
            public override void Dispose() { }

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
