//Assets\Colonization\Scripts\Characteristics\Perk\Ids\TargetOfPerkId.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class TargetOfPerkId : AIdType<TargetOfPerkId>
    {
        public const int Player = 0;
        public const int Warriors = 1;

        static TargetOfPerkId() => RunConstructor();
        private TargetOfPerkId() { }
    }
}
