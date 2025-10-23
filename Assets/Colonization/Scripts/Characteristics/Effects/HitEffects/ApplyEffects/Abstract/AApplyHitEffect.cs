using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
	public abstract class AApplyHitEffect : AHitEffect, IPerk
    {
        protected readonly int _targetAbility;
        protected readonly Id<TypeModifierId> _typeModifier;
        protected int _value;

        public int TargetAbility => _targetAbility;
        public Id<TypeModifierId> TypeModifier => _typeModifier;
        public int Value => _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AApplyHitEffect(int targetAbility, Id<TypeModifierId> typeModifier, int value)
        {
            _targetAbility = targetAbility;
            _typeModifier = typeModifier;
            _value = value;
        }
        public AApplyHitEffect(int targetAbility, Id<TypeModifierId> typeModifier) : this(targetAbility, typeModifier, 0) { }
    }
}
