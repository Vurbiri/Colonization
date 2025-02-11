//Assets\Colonization\Scripts\Characteristics\Effects\ReactiveEffect.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class ReactiveEffect : AReactiveItem<ReactiveEffect>, IPerk, IEquatable<ReactiveEffect>
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
        public int Duration => _duration;
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

        public bool UpdateDuration(ReactiveEffect other)
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
                Removing(); 
                return;
            }

            actionThisChange?.Invoke(this, TypeEvent.Change);
        }

        public int[] ToArray() => new int[] { _code, _targetAbility, _typeModifier.Value, _value, _duration };

        public override bool Equals(ReactiveEffect other)
        {
           if(other is null) return false;

            return _code == other._code;
        }
    }
}
