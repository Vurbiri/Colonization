//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\EffectCode.cs
using System;

namespace Vurbiri.Colonization.Characteristics
{
    public readonly struct EffectCode : IEquatable<EffectCode>
	{
        private const int MASK = 7, OFFSET_1 = 2;
        private const int OFFSET_2 = OFFSET_1 << 1, OFFSET_3 = OFFSET_2 + OFFSET_1;

        private readonly int _code;

        public EffectCode(int sourceId, int actorId, int skillId, int effectId)
		{
            _code = (sourceId & MASK) << OFFSET_3 | (actorId & MASK) << OFFSET_2 | (skillId & MASK) << OFFSET_1 | (effectId & MASK);
        }

        public EffectCode(EffectCode effectKey) => _code = effectKey._code;
        public EffectCode(int value) => _code = value;

        public int Value => _code;

        public int SourceId  => _code >> OFFSET_3 & MASK;
        public int ActorId   => _code >> OFFSET_2 & MASK;
        public int SkillId   => _code >> OFFSET_1 & MASK;
        public int EffectId  => _code & MASK;


        public static implicit operator EffectCode(int value) => new(value);
        public static implicit operator int(EffectCode effectKey) => effectKey._code;

        public override int GetHashCode() => _code;
        public bool Equals(EffectCode other) => _code == other._code;
        public override bool Equals(object obj) => obj is EffectCode other && _code == other._code;

        public static bool operator ==(EffectCode left, EffectCode right) => left._code == right._code;
        public static bool operator !=(EffectCode left, EffectCode right) => left._code != right._code;

        public override string ToString()
        {
            return $"Code: {_code} [SourceId: {SourceId}, ActorId: {ActorId}, SkillId: {SkillId}, EffectId: {EffectId}]";
        }

    }
}
