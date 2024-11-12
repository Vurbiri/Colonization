namespace Vurbiri.Colonization
{
    using System;
    using System.Collections.Generic;
    using Vurbiri.Reactive.Collections;

    public class Effect : AReactiveElement<Effect>, IPerk
    {
        private readonly int _targetAbility;
        private readonly Id<TypeOperationId> _typeOperation;
        private readonly int _value;
        private int _duration;
        
        public int TargetAbility => _targetAbility;
        public Id<TypeOperationId> TypeOperation => _typeOperation;
        public int Value => _value;
        public Chance Chance => 100;

        public Effect(int targetAbility, Id<TypeOperationId> typeOperation, int value, int duration)
        {
            _targetAbility = targetAbility;
            _typeOperation = typeOperation;
            _value = value;
            _duration = duration;
        }

        public Effect(EffectSettings settings)
        {
            _targetAbility = settings.TargetAbility;
            _typeOperation = settings.TypeOperation;
            _value = settings.Value;
            _duration = settings.Duration;
        }

        public Effect(IReadOnlyList<int> array)
        {
            if (array == null | array.Count != 4)
                throw new ArgumentOutOfRangeException(nameof(array));

            _targetAbility = array[0];
            _typeOperation = array[1];
            _value = array[2];
            _duration = array[3];
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

        public int[] ToArray() => new int[] { _targetAbility, _typeOperation.Value, _value, _duration };
    }
}
