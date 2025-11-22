using System.Collections;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Actor.AI;

namespace Vurbiri.Colonization
{
	public abstract class Commander<TAI> where TAI : Actor.AI
    {
        protected readonly Goals _goals = new();
        private readonly RandomSequenceList<int> _indexes;
        private readonly TAI[] _warriorsAI;

        protected Commander(int maxCount) 
		{
            _indexes = new(maxCount);
            _warriorsAI = new TAI[maxCount];
        }

        public IEnumerator Execution_Cn()
        {
            foreach (var index in _indexes)
                yield return GameContainer.Shared.StartCoroutine(_warriorsAI[index].Execution_Cn());
        }

        protected abstract TAI GetActorAI(Actor actor);

        protected void OnActor(Actor actor, TypeEvent type)
        {
            if (type == TypeEvent.Subscribe | type == TypeEvent.Add)
            {
                _warriorsAI[actor.Index] = GetActorAI(actor);
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
