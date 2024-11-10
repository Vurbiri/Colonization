namespace Vurbiri.Colonization
{
    public class EffectTypeId : AIdType<EffectTypeId>
    {
        public const int Negative = 0;
        public const int Positive = 1;

        static EffectTypeId() => RunConstructor();
        private EffectTypeId() { }
    }
}
