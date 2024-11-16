namespace Vurbiri.Colonization.Characteristics
{
    using Reactive.Collections;
    using System;
    using System.Collections.Generic;

    public class ReactiveEffect : AReactiveElement<ReactiveEffect>, IPerk
    {
        private readonly int _targetAbility;
        private readonly Id<TypeModifierId> _typeModifier;
        private readonly int _value;
        private int _duration;
        private readonly bool _isNegative;

        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;
        public Chance Chance => 100;
        public bool IsNegative => _isNegative;

        public ReactiveEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, bool isNegative)
        {
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
            _duration = duration;
            _isNegative = isNegative;
        }

        public ReactiveEffect(ReactiveEffect other)
        {
            _targetAbility = other._targetAbility;
            _typeModifier = other._typeModifier;
            _value = other._value;
            _duration = other._duration;
            _isNegative = other._isNegative;
        }

        public ReactiveEffect(IReadOnlyList<int> array)
        {
            if (array == null | array.Count != 5)
                throw new ArgumentOutOfRangeException(nameof(array));

            _targetAbility = array[0];
            _typeModifier = array[1];
            _value = array[2];
            _duration = array[3];
            _isNegative = array[4] > 0;
        }

        public void Next()
        {
            if (--_duration == 0)
            {
                Removing();
                return;
            }

            actionThisChange?.Invoke(this, Operation.Change);
        }

        public int[] ToArray() => new int[] { _targetAbility, _typeModifier.Value, _value, _duration, _isNegative ? 1 : 0 };

        
    }
}
