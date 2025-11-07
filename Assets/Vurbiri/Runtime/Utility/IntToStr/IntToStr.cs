namespace Vurbiri
{
	public static class IntToStr
	{
        private static readonly Settings s_settings;
        private static readonly string[] s_cache;

        public static int Min => s_settings.min;
        public static int Max => s_settings.max;
        public static int Count => s_settings.max - s_settings.min + 1;

        static IntToStr()
        {
            s_settings = (Settings)Storage.LoadObjectFromJsonResource(Settings.PATH, typeof(Settings));

            int count = s_settings.max - s_settings.min + 1;

            s_cache = new string[count];
            for (int i = 0, n = s_settings.min; i < count; i++, n++)
                s_cache[i] = n.ToString();
        }

        public static string ToStr(this int number)
        {
            string output;
            if (number >= s_settings.min && number <= s_settings.max)
                output = s_cache[number - s_settings.min];
            else
                output = number.ToString();
            return output;
        }

        // ****************** Nested **********************
        public struct Settings : System.IEquatable<Settings>
        {
#if UNITY_EDITOR
            public const string RESOURCE = "/Vurbiri/Runtime/Utility/IntToStr/Resources/";
            public const int MIN_LIMIT = -256, MAX_LIMIT = 512;
#endif
            public const string PATH = "IntToStr/Settings";

            public int min;
            public int max;

            public Settings(int min, int max)
            {
                this.min = min;
                this.max = max;
            }

            public readonly bool Equals(Settings other) => other.min == min && other.max == max;
        }
    }

      
}
