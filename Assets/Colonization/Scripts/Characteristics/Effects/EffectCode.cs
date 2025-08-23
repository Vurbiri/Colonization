using System;

namespace Vurbiri.Colonization.Characteristics
{
    public readonly struct EffectCode : IEquatable<EffectCode>
	{
        private const int OFFSET_1 = 3;
        private const int OFFSET_2 = OFFSET_1 << 1, OFFSET_3 = OFFSET_2 + OFFSET_1;
        private const int MASK = (1 << OFFSET_1) - 1;

        private readonly int _code;

        public EffectCode(int sourceType, int sourceId, int skillId, int effectId)
		{
            _code = (sourceType & MASK) << OFFSET_3 | (sourceId & MASK) << OFFSET_2 | (skillId & MASK) << OFFSET_1 | (effectId & MASK);
        }

        public EffectCode(EffectCode effectKey) => _code = effectKey._code;
        public EffectCode(int value) => _code = value;

        public int Value => _code;

        public int SourceType  => _code >> OFFSET_3 & MASK;
        public int SourceId    => _code >> OFFSET_2 & MASK;
        public int SkillId     => _code >> OFFSET_1 & MASK;
        public int EffectId    => _code & MASK;

        public static implicit operator int(EffectCode effectKey) => effectKey._code;

        public override int GetHashCode() => _code;
        public bool Equals(EffectCode other) => _code == other._code;
        public override bool Equals(object obj) => obj is EffectCode other && _code == other._code;

        public static bool operator ==(EffectCode left, EffectCode right) => left._code == right._code;
        public static bool operator !=(EffectCode left, EffectCode right) => left._code != right._code;

        public override string ToString()
        {
            return $"Code: {_code} [SourceType: {SourceType}, SourceId: {SourceId}, SkillId: {SkillId}, EffectId: {EffectId}]";
        }

    }
}
