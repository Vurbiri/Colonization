using System;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public partial class ReactiveEffect : AReactiveItem<ReactiveEffect>, IPerk, IEquatable<ReactiveEffect>
    {
        private readonly int _targetAbility;
        private readonly Id<TypeModifierId> _typeModifier;
        private readonly bool _fixed;
        private int _value;
        private int _duration;
        private int _skip;

        public readonly EffectCode code;

        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;
        public int Duration => _duration;
        public bool IsPositive => _value > 0;

        public ReactiveEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, bool isSkip, bool fix = false)
            : this(code, targetAbility, typeModifier, value, duration, isSkip ? 1 : 0) { }
        public ReactiveEffect(EffectCode code, int targetAbility, Id<TypeModifierId> typeModifier, int value, int duration, int skip)
        {
            this.code = code;
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
            _duration = duration;
            _skip = skip;
            _fixed = (code.SourceType == ActorTypeId.Warrior & code.SkillId == CONST.SPEC_SKILL_ID) | code == ReactiveEffectsFactory.WallEffectCode;
        }

        public bool Update(ReactiveEffect other, Func<IPerk, int> addPerk, out int delta)
        {
            delta = 0;
            if (code != other.code)
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

        public void Degrade(int duration, bool isPositive)
        {
            if (!_fixed & _value > 0 == isPositive)
                SetDuration(duration);
        }

        public void Degrade(int duration)
        {
            if (!_fixed)
                SetDuration(duration);
        }

        public void Next()
        {
            if (--_skip < 0)
                SetDuration(1);
        }

        public override bool Equals(ReactiveEffect other)
        {
           if(other is null) return false;

            return code == other.code;
        }
        public override void Dispose() {}

        public override int GetHashCode() => code;
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return obj is ReactiveEffect other && code == other.code;
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

        public static bool operator ==(ReactiveEffect effect, EffectCode code) => effect.code == code;
        public static bool operator !=(ReactiveEffect effect, EffectCode code) => effect.code != code;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetDuration(int remove)
        {
            _duration -= remove;
            if (_duration <= 0)
                Removing();
            else
                _eventChanged.Invoke(this, TypeEvent.Change);
        }

    }
}
