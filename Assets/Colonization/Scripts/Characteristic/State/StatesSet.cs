using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class StatesSet<TId> : IEnumerable<State<TId>> where TId : AStateId<TId>
    {
        private readonly State<TId>[] _states;
        private readonly int _count;

        public IReadOnlyReactiveValue<int> this[int index] => _states[index];
        public IReadOnlyReactiveValue<int> this[Id<TId> id] => _states[id.Value];

        public StatesSet(IdArray<TId, int> states)
        {
            _count = AStateId<TId>.Count;
            _states = new State<TId>[_count];

            for (int i = 0; i < _count; i++)
                _states[i] = new State<TId>(i, states[i]);
        }

        public bool IsGreater(Id<TId> stateId, int value) => _states[stateId.Value].NextValue > value;
        public bool IsLess(Id<TId> stateId, int value) => _states[stateId.Value].NextValue < value;

        public bool IsTrue(Id<TId> stateId) => _states[stateId.Value].NextValue > 0;

        public State<TId> GetState(Id<TId> stateId) => _states[stateId.Value];

        public int GetValue(Id<TId> stateId) => _states[stateId.Value].NextValue;

        public bool TryAddPerk(IPerk<TId> perk) => _states[perk.TargetAbility.Value].TryAddPerk(perk);

        public IEnumerator<State<TId>> GetEnumerator()
        {
            foreach(var state in _states)
                yield return state;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
