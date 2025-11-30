using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public struct MinMax
	{
		public static readonly MinMax empty = new(float.MaxValue, float.MinValue);

		public float min;
		public float max;

		public readonly float Delta { [Impl(256)] get => max - min; }

		[Impl(256)]
		public MinMax(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		[Impl(256)]
		public void Set(float value)
		{
			if (value < min) min = value;
			else
			if (value > max) max = value;
		}
	}
}
