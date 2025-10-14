using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class AAbility<TId> : Ability, IValueId<TId> where TId : AbilityId<TId>
    {
        public Id<TId> Id { [Impl(256)] get; }

        public AAbility(Id<TId> id, int value)
        {
            Id = id;
            _value = value;
        }

        public AAbility(AAbility<TId> other)
        {
            Id = other.Id;
            _value = other._value;
        }

        public abstract int AddModifier(IAbilityValue mod);
        public abstract int RemoveModifier(IAbilityValue mod);
    }
}
