namespace Vurbiri.Colonization
{
	public readonly struct ReturnSignal
	{
		public readonly bool result;
		public readonly WaitSignal signal;

		public ReturnSignal(WaitSignal signal)
		{
			this.signal = signal;
            this.result = true;
        }
        public ReturnSignal(bool result)
		{
			this.result = result;
			this.signal = null;
		}

		public static implicit operator bool(ReturnSignal signal) => signal.result;

        public static implicit operator ReturnSignal(bool result) => new(result);
        public static implicit operator ReturnSignal(WaitSignal signal) => new(signal);
    }
}
