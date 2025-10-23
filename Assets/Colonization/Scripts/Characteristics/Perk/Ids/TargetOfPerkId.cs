namespace Vurbiri.Colonization
{
    public abstract class TargetOfPerkId : IdType<TargetOfPerkId>
    {
        public const int Player = 0;
        public const int Warriors = 1;

        static TargetOfPerkId() => ConstructorRun();
    }
}
