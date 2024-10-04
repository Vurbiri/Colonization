namespace Vurbiri.Colonization
{
    public class SurfaceType : AIdType<SurfaceType>
    {
        public const int Village    = 0;
        public const int Field      = 1;
        public const int Crystals   = 2;
        public const int Mountain   = 3;
        public const int Forest     = 4;
        public const int Water      = 5;
        public const int Gate       = 6;

        static SurfaceType() { RunConstructor(); }
        private SurfaceType() { }
    }
}
