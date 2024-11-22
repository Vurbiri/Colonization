//Assets\Colonization\Scripts\Characteristics\Effects\Ids\TargetOfEffectId.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class TargetOfEffectId : AIdType<TargetOfEffectId>
    {
        public const int Target = 0;
        public const int Self = 1;

        static TargetOfEffectId() => RunConstructor();
        private TargetOfEffectId() { }
    }
}
