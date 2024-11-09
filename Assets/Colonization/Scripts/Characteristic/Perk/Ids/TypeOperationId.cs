namespace Vurbiri.Colonization
{
    public class TypeOperationId : AIdType<TypeOperationId>
    {
        public const int Addition = 0;
        public const int RandomAdd = 1;
        public const int Percent = 2;

        static TypeOperationId() => RunConstructor();
        private TypeOperationId() { }
    }
}
