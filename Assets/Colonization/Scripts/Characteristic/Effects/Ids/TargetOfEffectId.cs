namespace Vurbiri.Colonization
{

    public class TargetOfEffectId : AIdType<TargetOfEffectId>
    {
        public const int Enemy = 0;
        public const int Friend = 1;
        public const int Self = 2;

        static TargetOfEffectId() => RunConstructor();
        private TargetOfEffectId() { }
    }
}
