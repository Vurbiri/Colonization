//Assets\Vurbiri\Runtime\Utility\SysRandom.cs
using System;

namespace Vurbiri
{
    public static class SysRandom
	{
		private static readonly Random s_rnd = new();

        public static int Next() => s_rnd.Next();
        public static int Next(int maxValue) => s_rnd.Next(maxValue);
        public static int Next(int minValue, int maxValue) => s_rnd.Next(minValue, maxValue);

        public static void NextBytes(byte[] buffer) => s_rnd.NextBytes(buffer);
        public static void NextBytes(Span<byte> buffer) => s_rnd.NextBytes(buffer);

        public static double NextDouble() => s_rnd.NextDouble();
    }
}
