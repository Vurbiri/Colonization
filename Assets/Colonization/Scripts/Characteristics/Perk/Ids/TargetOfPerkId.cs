namespace Vurbiri.Colonization.Characteristics
{
    public abstract class TargetOfPerkId : IdType<TargetOfPerkId>
    {
        public const int Player = 0;
        public const int Warriors = 1;

        static TargetOfPerkId() => RunConstructor();
    }
}
