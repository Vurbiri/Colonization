namespace Vurbiri.UI
{
	public abstract class MBButtonId : IdType<MBButtonId>
    {
        public const int Ok = 0;
        public const int No = 1;
        public const int Cancel = 2;

        static MBButtonId() => ConstructorRun();
    }
}
