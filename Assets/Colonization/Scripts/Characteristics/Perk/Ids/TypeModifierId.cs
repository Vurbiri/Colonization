namespace Vurbiri.Colonization
{
    public abstract class TypeModifierId : IdType<TypeModifierId>
    {
        public const int BasePercent    = 0;
        public const int Addition       = 1;
        public const int TotalPercent   = 2;

        static TypeModifierId() => ConstructorRun();
    }
}
