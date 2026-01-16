using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	public struct MinMax
	{
		[UnityEngine.SerializeField] private float _min;
        [UnityEngine.SerializeField] private float _max;

        public static readonly MinMax empty = new(float.MaxValue, float.MinValue);

        public readonly float Min { [Impl(256)] get => _min; }
        public readonly float Max { [Impl(256)] get => _max; }
        public readonly float Delta { [Impl(256)] get => _max - _min; }

		[Impl(256)]
		public MinMax(float min, float max)
		{
            _min = min;
            _max = max;
		}

		[Impl(256)]
		public void Set(float value)
		{
			if (value < _min) _min = value;
			else
			if (value > _max) _max = value;
		}
    }
}
