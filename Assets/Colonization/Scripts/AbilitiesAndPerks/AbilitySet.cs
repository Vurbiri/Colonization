using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public class AbilitySet<T> : IEnumerable<Ability> where T : AAbilityId<T>
    {
        private readonly Ability[] _abilities;
        private int _count;
        private readonly int _capacity;

        public Ability this[Id<T> id] => _abilities[id.ToInt];

        public AbilitySet()
        {
            _capacity = AAbilityId<T>.Count;
            _count = 0;

            _abilities = new Ability[_capacity];
        }

        public AbilitySet(IdArray<T, int> abilities) : this()
        {
            for (int i = 0; i < _capacity; i++)
                Add(new(i, abilities[i]));
        }

        public AbilitySet(IEnumerable<Ability> abilities) : this()
        {
            foreach (Ability ability in abilities)
                Add(new(ability));
        }

        public bool TryAdd(Ability ability)
        {
            int index = ability.Id.ToInt;

            if (_abilities[index] != null)
                return false;

            _abilities[index] = ability;
            _count++;
            return true;
        }

        public void Add(Ability ability)
        {
            if (TryAdd(ability)) return;

            throw new Exception($"Абилка c Id = {ability.Id} уже была добавлена.");
        }

        public bool IsMore(Id<T> abilityId, int value = 0) => _abilities[abilityId.ToInt].NextValue > value;

        public int GetValue(Id<T> abilityId) => _abilities[abilityId.ToInt].NextValue;

        public bool TryAddPerk(IPerk<PlayerAbilityId> perk) => _abilities[perk.TargetAbility.ToInt].TryAddPerk(perk);

        public IEnumerator<Ability> GetEnumerator()
        {
            foreach(var ability in _abilities)
                yield return ability;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
