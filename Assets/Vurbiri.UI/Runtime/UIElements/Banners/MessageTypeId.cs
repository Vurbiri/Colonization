namespace Vurbiri.UI
{
	public abstract class MessageTypeId : IdType<MessageTypeId>
	{
		public const int Info	 = 0;
        public const int Warning = 1;
        public const int Error	 = 2;
        public const int Profit  = 3;
	}
}
