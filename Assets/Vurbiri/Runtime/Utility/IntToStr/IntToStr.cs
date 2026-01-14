using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public static class IntToStr
	{
		private static readonly Settings s_settings;
		private static readonly string[] s_cache;

		public static int Min { [Impl(256)] get => s_settings.min; }
		public static int Max { [Impl(256)] get => s_settings.max; }
		public static int Count { [Impl(256)] get => s_settings.max - s_settings.min + 1; }

		static IntToStr()
		{
			s_settings = (Settings)JsonResources.Load(Settings.PATH, typeof(Settings));

			int count = Count;

			s_cache = new string[count];
			for (int i = 0, n = s_settings.min; i < count; ++i, ++n)
				s_cache[i] = n.ToString();
		}

		[Impl(256)]
		public static string ToStr(this int number)
		{
			if (number >= s_settings.min && number <= s_settings.max)
				return s_cache[number - s_settings.min];

			return number.ToString();
		}

		// ****************** Nested **********************
		public struct Settings : System.IEquatable<Settings>
		{
			public const string PATH = "IntToStr/Settings";

			public int min;
			public int max;

			public Settings(int min, int max)
			{
				this.min = min;
				this.max = max;
			}

			public readonly bool Equals(Settings other) => other.min == min & other.max == max;
		}
	}

	  
}
