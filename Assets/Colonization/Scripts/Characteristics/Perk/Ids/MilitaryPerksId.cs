//Assets\Colonization\Scripts\Characteristics\Perk\Ids\MilitaryPerksId.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class MilitaryPerksId : APerkId<MilitaryPerksId>
    {
        public const int Defense_1 = 0;
        public const int Defense_2 = 1;
        public const int Attack_1 = 2;
        public const int Attack_2 = 3;

        static MilitaryPerksId() => RunConstructor();
    }
}
