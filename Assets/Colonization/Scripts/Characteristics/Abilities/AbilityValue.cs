namespace Vurbiri.Colonization.Characteristics
{
    using System;

    public readonly struct AbilityValue : IAbilityValue, IEquatable<AbilityValue>
    {
        private readonly int _value;
        private readonly int _chance;
        
        public readonly int Value => _value;
        public readonly Chance Chance => _chance;

        public AbilityValue(int value)
        {
            _value = value;
            _chance = 100;
        }

        public AbilityValue(int value, int chance)
        {
            _value = value;
            _chance = chance;
        }

        public static implicit operator AbilityValue(int value) => new(value);

        public readonly bool Equals(AbilityValue other) => _value == other._value & _chance == other._chance;
        public override readonly bool Equals(object obj) => obj is AbilityValue other && _value == other._value & _chance == other._chance;
        public override readonly int GetHashCode() => HashCode.Combine(_value, _chance);
    }
}
