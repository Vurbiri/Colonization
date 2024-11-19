using System;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReactiveEffect : AReactiveElement<ReactiveEffect>, IPerk
    {
        private readonly EffectCode _code;
        private readonly int _targetAbility;
        private readonly Id<TypeModifierId> _typeModifier;
        private readonly int _value;
        private int _duration;

        public EffectCode Code => _code;
        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;
        public Chance Chance => 100;
        public bool IsNegative => _value < 0;

        public ReactiveEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration)
        {
            _code = code;
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
            _duration = duration;
        }

        public ReactiveEffect(IReadOnlyList<int> array)
        {
            if (array == null | array.Count != 5)
                throw new ArgumentOutOfRangeException(nameof(array));
            int i = 0;
            _code = array[i++];
            _targetAbility = array[i++];
            _typeModifier = array[i++];
            _value = array[i++];
            _duration = array[i++];
        }

        public void Subscribe(Action<ReactiveEffect, TypeEvent> action)
        {
            actionThisChange -= action ?? throw new ArgumentNullException("action");
            actionThisChange += action;

            action(this, TypeEvent.Subscribe);
        }

        public bool Update(ReactiveEffect other)
        {
            if (_code != other._code)
                return false;

            if(_duration != other._duration)
            {
                _duration = other._duration;
                actionThisChange?.Invoke(this, TypeEvent.Change);
            }

            return true;
        }

        public void Next()
        {
            if (--_duration == 0)
            {
                Removing(); return;
            }

            actionThisChange?.Invoke(this, TypeEvent.Change);
        }

        public int[] ToArray() => new int[] { _code, _targetAbility, _typeModifier.Value, _value, _duration };

        public override int GetHashCode() => _code.GetHashCode();
        public bool Equals(ReactiveEffect other) => other is not null && _code == other._code;
        public override bool Equals(object obj) => obj is ReactiveEffect other && _code == other._code;

        public static bool operator ==(ReactiveEffect left, ReactiveEffect right) => left._code == right._code;
        public static bool operator !=(ReactiveEffect left, ReactiveEffect right) => left._code != right._code;
    }
}
