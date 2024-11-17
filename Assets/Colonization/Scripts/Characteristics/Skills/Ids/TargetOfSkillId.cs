namespace Vurbiri.Colonization.Characteristics
{
    public class TargetOfSkillId : AIdType<TargetOfSkillId>
    {
        public const int Enemy = 0;
        public const int Self = 1;
        public const int Friend = 2;

        static TargetOfSkillId() => RunConstructor();
        private TargetOfSkillId() { }
    }
}
