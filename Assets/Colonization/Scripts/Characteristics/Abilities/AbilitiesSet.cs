using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReadOnlyAbilities<TId> : IReadOnlyList<Ability> where TId : AbilityId<TId>
    {
        protected readonly IdArray<TId, AAbility<TId>> _abilities = new();

        public Ability this[int index] { [Impl(256)] get => _abilities[index]; }
        public Ability this[Id<TId> id] { [Impl(256)] get => _abilities[id]; }

        public int Count { [Impl(256)] get => AbilityId<TId>.Count; }

        [Impl(256)] public bool IsGreater(Id<TId> stateId, int value) => _abilities[stateId].Value > value;
        [Impl(256)] public bool IsGreaterOrEqual(Id<TId> stateId, int value) => _abilities[stateId].Value >= value;
        [Impl(256)] public bool IsLess(Id<TId> stateId, int value) => _abilities[stateId].Value < value;
        [Impl(256)] public bool IsLessOrEqual(Id<TId> stateId, int value) => _abilities[stateId].Value <= value;

        [Impl(256)] public bool IsTrue(Id<TId> stateId) => _abilities[stateId].IsTrue;
        [Impl(256)] public bool IsFalse(Id<TId> stateId) => _abilities[stateId].IsFalse;

        public IEnumerator<Ability> GetEnumerator() => _abilities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _abilities.GetEnumerator();
    }


    public class AbilitiesSet<TId> : ReadOnlyAbilities<TId> where TId : AbilityId<TId>
    {
        public AbilitiesSet(IdArray<TId, int> states, PerkTree perks)
        {
            for (int i = 0; i < AbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);

            perks.Subscribe(OnPerks);
        }

        public AbilitiesSet(IdArray<TId, int> states, int shift, int maxIndexShift)
        {
            int i;
            for (i = 0; i <= maxIndexShift; i++)
                _abilities[i] = new Ability<TId>(i, states[i] << shift);
            for (; i < AbilityId<TId>.Count; i++)
                _abilities[i] = new Ability<TId>(i, states[i]);
        }

        public BooleanAbility<TId> ReplaceToBoolean(Id<TId> id) => ReplaceTo(id, new BooleanAbility<TId>(_abilities[id]));
        public ChanceAbility<TId> ReplaceToChance(Id<TId> id, Ability ratioA, Ability ratioB)
        {
            return ReplaceTo(id, new ChanceAbility<TId>(_abilities[id], ratioA, ratioB));
        }
        public SubAbility<TId> ReplaceToSub(Id<TId> id, Id<TId> max, Id<TId> restore)
        {
            return ReplaceTo(id, new SubAbility<TId>(_abilities[id], _abilities[max], _abilities[restore]));
        }
        public void ReplaceToDependent(Id<TId> id, ReactiveValue<int> remove, ReactiveValue<int> add)
        {
            _abilities[id] = new DependentAbility<TId>(_abilities[id], remove, add);
        }

        [Impl(256)] public int AddPerk(IPerk perk) => _abilities[perk.TargetAbility].AddModifier(perk);
        [Impl(256)] public int RemovePerk(IPerk perk) => _abilities[perk.TargetAbility].RemoveModifier(perk);

        [Impl(256)] private T ReplaceTo<T>(Id<TId> id, T newAbility) where T : AAbility<TId>
        {
            _abilities[id] = newAbility;
            return newAbility;
        }

        private void OnPerks(Perk perk)
        {
            if (perk.TargetObject == TargetOfPerkId.Player)
                _abilities[perk.TargetAbility].AddModifier(perk);
        }
    }
}
