//Assets\Colonization\Scripts\Characteristics\Effects\ReactiveEffect.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReactiveEffect : AReactiveItem<ReactiveEffect>, IPerk, IEquatable<ReactiveEffect>, IArrayable
    {
        private readonly EffectCode _code;
        private readonly int _targetAbility;
        private readonly Id<TypeModifierId> _typeModifier;
        private int _value;
        private int _duration;

        public EffectCode Code => _code;
        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;
        public int Duration => _duration;
        public bool IsPositive => _value > 0;

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
            _duration = array[i];
        }

        public bool Update(ReactiveEffect other, Func<IPerk, int> addPerk, out int delta)
        {
            delta = 0;
            if (_code != other._code)
                return false;

            bool changeDuration = _duration < other._duration;
            bool changeValue = _value != other._value;

            if(!changeDuration & !changeValue)
                return true;

            if (changeDuration)
                _duration = other._duration;

            if (changeValue)
            {
                delta = addPerk(other - this);
                _value = other._value;
            }

            _subscriber.Invoke(this, TypeEvent.Change);

            return true;
        }

        public void Next()
        {
            if (--_duration == 0)
            {
                Removing(); 
                return;
            }

            _subscriber.Invoke(this, TypeEvent.Change);
        }

        #region IArrayable
        private const int SIZE_ARRAY = 5;
        public int[] ToArray() => new int[] { _code, _targetAbility, _typeModifier.Value, _value, _duration };
        public int[] ToArray(int[] array)
        {
            if (array == null || array.Length != SIZE_ARRAY)
                return ToArray();

            int i = 0;
            array[i++] = _code; array[i++] = _targetAbility; array[i++] = _typeModifier.Value; array[i++] = _value; array[i] = _duration;
            return array;
        }
        #endregion

        public override bool Equals(ReactiveEffect other)
        {
           if(other is null) return false;

            return _code == other._code;
        }

        public static Perk operator-(ReactiveEffect left, ReactiveEffect right)
        {
            if(left == null | right == null)
                return null;
            if(left._targetAbility != right._targetAbility | left._typeModifier != right._typeModifier)
                return null;

            return new(right._targetAbility, right._typeModifier, left._value - right._value);
        }
        public static Perk operator +(ReactiveEffect left, ReactiveEffect right)
        {
            if (left == null | right == null)
                return null;
            if (left._targetAbility != right._targetAbility | left._typeModifier != right._typeModifier)
                return null;

            return new(right._targetAbility, right._typeModifier, left._value + right._value);
        }
    }
}
