using System;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public partial class ReactiveEffect : AReactiveItem<ReactiveEffect>, IPerk, IEquatable<ReactiveEffect>
    {
        private readonly EffectCode _code;
        private readonly int _targetAbility;
        private readonly Id<TypeModifierId> _typeModifier;
        private int _value;
        private int _duration;
        private int _skip;

        public EffectCode Code => _code;
        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;
        public int Duration => _duration;
        public bool IsPositive => _value > 0;

        public ReactiveEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, bool isSkip)
                       : this(code, targetAbility, typeModifier, value, duration, isSkip ? 1 : 0) { }
        public ReactiveEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, int skip)
        {
            _code = code;
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
            _duration = duration;
            _skip = skip;
        }

        public bool Update(ReactiveEffect other, Func<IPerk, int> addPerk, out int delta)
        {
            delta = 0;
            if (_code != other._code)
                return false;

            bool changeDuration = _duration < other._duration;
            bool changeValue = _value != other._value;

            if (_skip < other._skip)
                _skip = other._skip;

            if (!changeDuration & !changeValue)
                return true;

            if (changeDuration)
                _duration = other._duration;

            if (changeValue)
            {
                delta = addPerk(other - this);
                _value = other._value;
            }

            _eventChanged.Invoke(this, TypeEvent.Change);

            return true;
        }

        public void Next()
        {
            if (_skip --> 0)
                return;

            if (--_duration == 0)
            {
                Removing(); 
                return;
            }

            _eventChanged.Invoke(this, TypeEvent.Change);
        }

        public override bool Equals(ReactiveEffect other)
        {
           if(other is null) return false;

            return _code == other._code;
        }
        public override void Dispose() {}

        public static Effect operator -(ReactiveEffect left, ReactiveEffect right)
        {
            if(left == null | right == null)
                return null;
            if(left._targetAbility != right._targetAbility | left._typeModifier != right._typeModifier)
                return null;

            return new(right._targetAbility, right._typeModifier, left._value - right._value);
        }
        public static Effect operator +(ReactiveEffect left, ReactiveEffect right)
        {
            if (left == null | right == null)
                return null;
            if (left._targetAbility != right._targetAbility | left._typeModifier != right._typeModifier)
                return null;

            return new(right._targetAbility, right._typeModifier, left._value + right._value);
        }
    }
}
