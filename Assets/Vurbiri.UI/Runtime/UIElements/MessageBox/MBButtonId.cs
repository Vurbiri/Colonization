namespace Vurbiri.UI
{
	public abstract class MBButtonId : IdType<MBButtonId>
    {
        public const int Ok = 0;
        public const int No = 1;

        static MBButtonId() => ConstructorRun();
    }
}
