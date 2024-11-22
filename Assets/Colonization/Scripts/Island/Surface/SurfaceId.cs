//Assets\Colonization\Scripts\Island\Surface\SurfaceId.cs
namespace Vurbiri.Colonization
{
    public class SurfaceId : AIdType<SurfaceId>
    {
        public const int Village    = 0;
        public const int Field      = 1;
        public const int Crystals   = 2;
        public const int Mountain   = 3;
        public const int Forest     = 4;
        public const int Water      = 5;
        public const int Gate       = 6;

        static SurfaceId() => RunConstructor();
        private SurfaceId() { }
    }
}
