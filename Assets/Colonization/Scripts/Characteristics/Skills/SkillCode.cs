using System;

namespace Vurbiri.Colonization.Characteristics
{
    using static EffectCode;

    public readonly struct SkillCode : IEquatable<EffectCode>
    {
        private const int INV_MASK = ~MASK;

        public readonly int code;

        public SkillCode(int sourceType, int sourceId, int skillId)
        {
            code = (sourceType & MASK) << OFFSET_3 | (sourceId & MASK) << OFFSET_2 | (skillId & MASK) << OFFSET_1;
        }

        public SkillCode(int value) => code = value;

        public int SourceType => code >> OFFSET_3 & MASK;
        public int SourceId => code >> OFFSET_2 & MASK;
        public int SkillId => code >> OFFSET_1 & MASK;

        public static implicit operator int(SkillCode skillKey) => skillKey.code;

        public static implicit operator EffectCode(SkillCode skillKey) => new(skillKey.code);
        public static implicit operator SkillCode(EffectCode effectKey) => new(effectKey.code & INV_MASK);

        public override int GetHashCode() => code;
        public bool Equals(EffectCode effect) => code == (effect.code & INV_MASK);
        public override bool Equals(object obj) => obj is EffectCode effect && code == (effect.code & INV_MASK);

        public static bool operator ==(SkillCode skill, EffectCode effect) => skill.code == (effect.code & INV_MASK);
        public static bool operator !=(SkillCode skill, EffectCode effect) => skill.code != (effect.code & INV_MASK);

        public override string ToString()
        {
            return $"Code: {code} [SourceType: {SourceType}, SourceId: {SourceId}, SkillId: {SkillId}]";
        }

    }
}
