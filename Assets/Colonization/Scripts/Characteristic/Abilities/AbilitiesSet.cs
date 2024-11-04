using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class AbilitiesSet<TId> : IReadOnlyList<IReadOnlyReactiveValue<int>> where TId : AAbilityd<TId>
    {
        private readonly IdArray<TId, Ability<TId>> _abilities = new();

        public IReadOnlyReactiveValue<int> this[int index] => _abilities[index];
        public IReadOnlyReactiveValue<int> this[Id<TId> id] => _abilities[id];

        public int Count => AAbilityd<TId>.Count;

        public AbilitiesSet(IdArray<TId, int> states)
        {
            for (int i = 0; i < AAbilityd<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);
        }

        public bool IsGreater(Id<TId> stateId, int value) => _abilities[stateId].NextValue > value;
        public bool IsLess(Id<TId> stateId, int value) => _abilities[stateId].NextValue < value;

        public bool IsTrue(Id<TId> stateId) => _abilities[stateId].NextValue > 0;

        public Ability<TId> GetState(Id<TId> stateId) => _abilities[stateId];

        public int GetValue(Id<TId> stateId) => _abilities[stateId].NextValue;

        public bool TryAddPerk(IPerk<TId> perk) => _abilities[perk.TargetAbility].TryAddPerk(perk);

        public IEnumerator<IReadOnlyReactiveValue<int>> GetEnumerator()
        {
            for (int i = 0; i < AAbilityd<TId>.Count; i++)
                yield return _abilities[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
