namespace Vurbiri.Colonization
{
    public class TypePerksId : AIdType<TypePerksId>
    {
        public const int Economic = 0;
        public const int Military = 1;

        static TypePerksId() => RunConstructor();
        private TypePerksId() { }
    }
}
