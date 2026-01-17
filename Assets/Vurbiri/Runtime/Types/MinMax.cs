using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	public struct MinMax
	{
		[UnityEngine.SerializeField] private float _min;
		[UnityEngine.SerializeField] private float _max;

		public readonly static MinMax Empty = new() { _min = float.MaxValue, _max = float.MinValue };

		public readonly float Min { [Impl(256)] get => _min; }
		public readonly float Max { [Impl(256)] get => _max; }
		public readonly float Delta { [Impl(256)] get => _max - _min; }
		public readonly float Avg { [Impl(256)] get => (_min + _max) * 0.5f; }

		[Impl(256)] public MinMax(float min, float max)
		{
			if (max > min) { _min = min; _max = max; }
			else           { _min = max; _max = min; }
		}

		[Impl(256)] public void Set(float value)
		{
			if (value < _min) _min = value;
			else
			if (value > _max) _max = value;
		}

		[Impl(256)] public override readonly string ToString() => $"[{_min}, {_max}]";
	}
}
