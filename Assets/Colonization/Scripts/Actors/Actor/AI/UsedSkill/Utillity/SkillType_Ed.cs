#if UNITY_EDITOR

using Vurbiri;

namespace VurbiriEditor.Colonization
{
	public abstract class SkillType_Ed : IdType<SkillType_Ed>
	{
        public const int Attacks   = 0;
        public const int SelfBuffs = 1;
        public const int Buffs     = 2;
        public const int Debuffs   = 3;
    }
}

#endif
