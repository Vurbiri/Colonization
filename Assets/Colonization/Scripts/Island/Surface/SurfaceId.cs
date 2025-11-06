namespace Vurbiri.Colonization
{
    public abstract class SurfaceId : IdType<SurfaceId>
    {
        public const int Village    = 0;
        public const int Field      = 1;
        public const int Forest     = 2;
        public const int Mountain   = 3;
        public const int Crystals   = 4;
        public const int Water      = 5;
        public const int Gate       = 6;

        [NotId] public const int CountGround = 5;
    }
}
