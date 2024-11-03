using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class StatesSet<TId> : IReadOnlyList<IReadOnlyReactiveValue<int>> where TId : AStateId<TId>
    {
        private readonly IdArray<TId, State<TId>> _states = new();

        public IReadOnlyReactiveValue<int> this[int index] => _states[index];
        public IReadOnlyReactiveValue<int> this[Id<TId> id] => _states[id];

        public int Count => AStateId<TId>.Count;

        public StatesSet(IdArray<TId, int> states)
        {
            for (int i = 0; i < AStateId<TId>.Count; i++)
                _states[i] = new State<TId>(i, states[i]);
        }

        public bool IsGreater(Id<TId> stateId, int value) => _states[stateId].NextValue > value;
        public bool IsLess(Id<TId> stateId, int value) => _states[stateId].NextValue < value;

        public bool IsTrue(Id<TId> stateId) => _states[stateId].NextValue > 0;

        public State<TId> GetState(Id<TId> stateId) => _states[stateId];

        public int GetValue(Id<TId> stateId) => _states[stateId].NextValue;

        public bool TryAddPerk(IPerk<TId> perk) => _states[perk.TargetAbility].TryAddPerk(perk);

        public IEnumerator<IReadOnlyReactiveValue<int>> GetEnumerator()
        {
            for (int i = 0; i < AStateId<TId>.Count; i++)
                yield return _states[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
