namespace Vurbiri.Colonization
{
    public abstract class MixerId : IdType<MixerId>
	{
		public const int Master = 0;
        public const int Music = 1;
        public const int SFX = 2;
        public const int Ambient = 3;

        static MixerId() => RunConstructor();
	}
}
