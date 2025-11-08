using System;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public partial class ReactiveEffect : AReactiveItem<ReactiveEffect>, IPerk, IEquatable<ReactiveEffect>
    {
        private readonly int _targetAbility;
        private readonly Id<TypeModifierId> _typeModifier;
        private readonly EffectCode _code;
        private readonly bool _fixed;
        private int _value;
        private int _duration;
        private int _skip;
        
        public int TargetAbility { [Impl(256)] get => _targetAbility; }
        public Id<TypeModifierId> TypeModifier { [Impl(256)] get => _typeModifier; }
        public int Value { [Impl(256)] get => _value; }
        public EffectCode Code { [Impl(256)] get => _code; }
        public int Duration { [Impl(256)] get => _duration; }
        public bool IsPositive { [Impl(256)] get => _value > 0; }

        public ReactiveEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, bool isSkip, bool fix = false)
            : this(code, targetAbility, typeModifier, value, duration, isSkip ? 1 : 0) { }
        public ReactiveEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, int skip)
        {
            _code = code;
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
            _duration = duration;
            _skip = skip;
            _fixed = (code.SourceType == ActorTypeId.Warrior && code.SkillId == CONST.SPEC_SKILL_ID) || code == ReactiveEffectsFactory.WallEffectCode;
        }

        public bool TryUpdate(ReactiveEffect other, Func<IPerk, int> addPerk, out int delta)
        {
            bool result = _code == other._code; delta = 0;

            if (result)
            {
                bool changeDuration = _duration < other._duration;
                bool changeValue = _value != other._value;

                if (_skip < other._skip)
                    _skip = other._skip;

                if (changeDuration | changeValue)
                {
                    if (changeDuration)
                        _duration = other._duration;

                    if (changeValue)
                    {
                        delta = addPerk(other - this);
                        _value = other._value;
                    }

                    _eventChanged.Invoke(this, TypeEvent.Change);
                }
            }
            return result;
        }

        [Impl(256)] public void Degrade(int duration, bool isPositive)
        {
            if (!_fixed & _value > 0 == isPositive)
                SetDuration(duration);
        }

        [Impl(256)] public void Degrade(int duration)
        {
            if (!_fixed)
                SetDuration(duration);
        }

        [Impl(256)] public void Next()
        {
            if (--_skip < 0)
                SetDuration(1);
        }

        [Impl(256)] public override bool Equals(ReactiveEffect other) => other is not null && _code == other._code;
        [Impl(256)] public override void Dispose() {}

        [Impl(256)] public override int GetHashCode() => _code;
        [Impl(256)] public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || (obj is ReactiveEffect other && _code == other._code);
        }

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

        [Impl(256)] public static bool operator ==(ReactiveEffect effect, EffectCode code) => effect._code == code;
        [Impl(256)] public static bool operator !=(ReactiveEffect effect, EffectCode code) => effect._code != code;

        [Impl(256)] private void SetDuration(int remove)
        {
            _duration -= remove;
            if (_duration <= 0)
                Removing();
            else
                _eventChanged.Invoke(this, TypeEvent.Change);
        }

    }
}
