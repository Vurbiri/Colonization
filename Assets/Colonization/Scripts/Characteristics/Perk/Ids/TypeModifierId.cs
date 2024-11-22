//Assets\Colonization\Scripts\Characteristics\Perk\Ids\TypeModifierId.cs
namespace Vurbiri.Colonization.Characteristics
{
    public class TypeModifierId : AIdType<TypeModifierId>
    {
        public const int Addition = 0;
        public const int RandomAdd = 1;
        public const int Percent = 2;

        static TypeModifierId() => RunConstructor();
        private TypeModifierId() { }
    }
}
