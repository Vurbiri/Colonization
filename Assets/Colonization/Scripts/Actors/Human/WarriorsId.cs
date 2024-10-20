namespace Vurbiri.Colonization
{
    public class WarriorsId : AIdType<WarriorsId>
    {
        public const int Militia = 0;
        public const int Solder = 1;
        public const int Wizard = 2;
        public const int Saboteur = 3;
        public const int Knight = 4;

        static WarriorsId()
        {
            RunConstructor();
        }
        private WarriorsId() { }
    }
}
