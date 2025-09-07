using System;

namespace Vurbiri.Colonization.Characteristics
{
    public readonly struct EffectCode : IEquatable<EffectCode>
	{
        public const int OFFSET_1 = 3;
        public const int OFFSET_2 = OFFSET_1 << 1, OFFSET_3 = OFFSET_2 + OFFSET_1;
        public const int MASK = (1 << OFFSET_1) - 1;

        public readonly int code;

        public EffectCode(int sourceType, int sourceId, int skillId, int effectId)
		{
            code = (sourceType & MASK) << OFFSET_3 | (sourceId & MASK) << OFFSET_2 | (skillId & MASK) << OFFSET_1 | (effectId & MASK);
        }

        public EffectCode(int value) => code = value;

        public int SourceType  => code >> OFFSET_3 & MASK;
        public int SourceId    => code >> OFFSET_2 & MASK;
        public int SkillId     => code >> OFFSET_1 & MASK;
        public int EffectId    => code & MASK;

        public static implicit operator int(EffectCode effectKey) => effectKey.code;

        public override int GetHashCode() => code;
        public bool Equals(EffectCode other) => code == other.code;
        public override bool Equals(object obj) => obj is EffectCode other && code == other.code;

        public static bool operator ==(EffectCode left, EffectCode right) => left.code == right.code;
        public static bool operator !=(EffectCode left, EffectCode right) => left.code != right.code;

        public override string ToString()
        {
            return $"Code: {code} [SourceType: {SourceType}, SourceId: {SourceId}, SkillId: {SkillId}, EffectId: {EffectId}]";
        }

    }
}
