using System.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Commander : Counselor
        {
            private readonly RandomSequenceList<WarriorAI> _warriorsAI = new(CONST.DEFAULT_MAX_WARRIOR);
            
            public Commander(AIController parent) : base(parent)
            {
                Human.Actors.Subscribe(OnActor);
            }

            public override IEnumerator Execution_Cn()
            {
                foreach (var warrior in _warriorsAI)
                    yield return StartCoroutine(warrior.Execution_Cn());
            }

            private void OnActor(Actor actor, TypeEvent type)
            {
                if (type == TypeEvent.Subscribe | type == TypeEvent.Add)
                    _warriorsAI.Add(WarriorAI.Create(actor));
                else if (type == TypeEvent.Remove)
                    _warriorsAI.Remove(actor, WarriorAI.Equals);
            }
        }
    }
}
