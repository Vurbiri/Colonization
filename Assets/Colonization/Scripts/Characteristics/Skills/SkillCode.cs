using System;
using static Vurbiri.Colonization.EffectCode;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public readonly struct SkillCode : IEquatable<EffectCode>
    {
        private const int INV_MASK = ~MASK;

        public readonly int code;

        public SkillCode(int sourceType, int sourceId, int skillId)
        {
            code = (sourceType & MASK) << OFFSET_3 | (sourceId & MASK) << OFFSET_2 | (skillId & MASK) << OFFSET_1;
        }

        public SkillCode(int value) => code = value;

        public int SourceType { [Impl(256)] get => code >> OFFSET_3 & MASK; }
        public int SourceId   { [Impl(256)] get => code >> OFFSET_2 & MASK; }
        public int SkillId    { [Impl(256)] get => code >> OFFSET_1 & MASK; }

        [Impl(256)] public static int Convert(int actorType, int actorId, int skillId) => (actorType & MASK) << OFFSET_3 | (actorId & MASK) << OFFSET_2 | (skillId & MASK) << OFFSET_1;

        [Impl(256)] public static implicit operator int(SkillCode skillKey) => skillKey.code;

        [Impl(256)] public static implicit operator EffectCode(SkillCode skillKey) => new(skillKey);
        [Impl(256)] public static implicit operator SkillCode(EffectCode effectKey) => new(effectKey.code & INV_MASK);

        public override int GetHashCode() => code;
        public bool Equals(EffectCode effect) => code == (effect.code & INV_MASK);
        public override bool Equals(object obj) => (obj is SkillCode skill && code == skill.code) || (obj is EffectCode effect && code == (effect.code & INV_MASK));

        [Impl(256)] public static bool operator ==(SkillCode skill, EffectCode effect) => skill.code == (effect.code & INV_MASK);
        [Impl(256)] public static bool operator !=(SkillCode skill, EffectCode effect) => skill.code != (effect.code & INV_MASK);

        public override string ToString()
        {
            return $"Code: {code} [SourceType: {SourceType}, SourceId: {SourceId}, SkillId: {SkillId}]";
        }

    }
}
