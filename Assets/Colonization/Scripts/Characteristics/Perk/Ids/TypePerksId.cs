//Assets\Colonization\Scripts\Characteristics\Perk\Ids\TypePerksId.cs
namespace Vurbiri.Colonization.Characteristics
{
    public abstract class TypePerksId : IdType<TypePerksId>
    {
        public const int Economic = 0;
        public const int Military = 1;

        static TypePerksId() => RunConstructor();
    }
}
