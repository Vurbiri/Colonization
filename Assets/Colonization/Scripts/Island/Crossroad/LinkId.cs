namespace Vurbiri.Colonization
{
    public class LinkId : IdType<LinkId>
    {
        public const int DR_UL = 0;
        public const int DL_UR = 1;
        public const int DD_UU = 2;

        static LinkId() => ConstructorRun();
        private LinkId() { }
    }
}
