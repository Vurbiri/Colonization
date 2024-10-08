using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public class StatesSet<TId> : IEnumerable<State<TId>> where TId : AStateId<TId>
    {
        private readonly State<TId>[] _states;
        private readonly int _count;

        public State<TId> this[Id<TId> id] => _states[id.ToInt];

        public StatesSet(IdArray<TId, int> states)
        {
            _count = AStateId<TId>.Count;
            _states = new State<TId>[_count];

            for (int i = 0; i < _count; i++)
                _states[i] = new State<TId>(i, states[i]);
        }

        public bool IsMore(Id<TId> stateId, int value = 0) => _states[stateId.ToInt].NextValue > value;

        public int GetValue(Id<TId> stateId) => _states[stateId.ToInt].NextValue;

        public bool TryAddPerk(IPerk<TId> perk) => _states[perk.TargetAbility.ToInt].TryAddPerk(perk);

        public IEnumerator<State<TId>> GetEnumerator()
        {
            foreach(var state in _states)
                yield return state;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
