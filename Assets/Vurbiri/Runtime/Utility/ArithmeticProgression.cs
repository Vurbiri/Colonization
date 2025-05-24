namespace Vurbiri
{
    public class ArithmeticProgression
	{
		private readonly int _a0, _d;
		
		public ArithmeticProgression(int a0, int d)
		{
            Throw.IfZero(d);

            _a0 = a0; _d = d;
		}

        public ArithmeticProgression(int d) : this(0, d) { }

        public int Item(int n) => _a0 + _d * n;
        public int MinStep(int value) => (value - _a0) / _d;
        public int Sum(int n) => ((_a0 << 1) + _d * n) * (n + 1) >> 1;
    }

    public class ArithmeticProgressionF
    {
        private readonly float _a0, _d;

        public ArithmeticProgressionF(float a0, float d)
        {
            Throw.IfZero(d);

            _a0 = a0; _d = d;
        }
        public ArithmeticProgressionF(float d) : this(0f, d) { }

        public float Item(int n) => _a0 + _d * n;
        public int MinStep(float value) => (int)((value - _a0) / _d);
        public float Sum(int n) => (2f * _a0 + _d * n) * (n + 1) / 2f;
    }
}
