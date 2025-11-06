using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public readonly struct EffectCode : IEquatable<EffectCode>
	{
        public const int OFFSET_1 = 3;
        public const int OFFSET_2 = OFFSET_1 << 1, OFFSET_3 = OFFSET_2 + OFFSET_1;
        public const int MASK = (1 << OFFSET_1) - 1;

        public readonly int code;

        [Impl(256)] public EffectCode(int sourceType, int sourceId, int skillId, int effectId)
		{
            code = (sourceType & MASK) << OFFSET_3 | (sourceId & MASK) << OFFSET_2 | (skillId & MASK) << OFFSET_1 | (effectId & MASK);
        }
        [Impl(256)] public EffectCode(SkillCode skillKey) => code = skillKey.code;
        [Impl(256)] public EffectCode(int value) => code = value;

        public int SourceType { [Impl(256)] get => code >> OFFSET_3 & MASK; }
        public int SourceId   { [Impl(256)] get => code >> OFFSET_2 & MASK; }
        public int SkillId    { [Impl(256)] get => code >> OFFSET_1 & MASK; }
        public int EffectId   { [Impl(256)] get => code & MASK; }

        public static implicit operator int(EffectCode effectKey) => effectKey.code;

        public override int GetHashCode() => code;
        public bool Equals(EffectCode other) => code == other.code;
        public override bool Equals(object obj) => obj is EffectCode other && code == other.code;

        [Impl(256)] public static bool operator ==(EffectCode left, EffectCode right) => left.code == right.code;
        [Impl(256)] public static bool operator !=(EffectCode left, EffectCode right) => left.code != right.code;

        public override string ToString()
        {
            return $"Code: {code} [SourceType: {SourceType}, SourceId: {SourceId}, SkillId: {SkillId}, EffectId: {EffectId}]";
        }

    }
}
