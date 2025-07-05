namespace Vurbiri.Colonization
{
    public abstract class TypeOfPerksId : IdType<TypeOfPerksId>
    {
        public const int Economic = 0;
        public const int Military = 1;

        static TypeOfPerksId() => ConstructorRun();
    }
}
