using System.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Commander : Counselor
        {
            private readonly RandomSequenceList<int> _indexes = new(CONST.DEFAULT_MAX_WARRIOR);
            private readonly WarriorAI[] _warriorsAI = new WarriorAI[CONST.DEFAULT_MAX_WARRIOR];
            private readonly Actor.AI.Goals _goals = new();

            public Commander(AIController parent) : base(parent)
            {
                Human.Actors.Subscribe(OnActor);
            }

            public override IEnumerator Execution_Cn()
            {
                foreach (var index in _indexes)
                    yield return StartCoroutine(_warriorsAI[index].Execution_Cn());
            }

            private void OnActor(Actor actor, TypeEvent type)
            {
                
                if (type == TypeEvent.Subscribe | type == TypeEvent.Add)
                {
                    _warriorsAI[actor.Index] = new WarriorAI(actor, _goals);
                    _indexes.Add(actor.Index);
                }
                else if (type == TypeEvent.Remove)
                {
                    _warriorsAI[actor.Index].Dispose();
                    _warriorsAI[actor.Index] = null;
                    _indexes.Remove(actor.Index);
                }
            }
        }
    }
}
