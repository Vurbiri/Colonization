//Assets\Vurbiri\Runtime\Utilities\SysRandom.cs
using System;

namespace Vurbiri
{
    public static class SysRandom
	{
		private static readonly Random _rnd = new();

        public static int Next() => _rnd.Next();
        public static int Next(int maxValue) => _rnd.Next(maxValue);
        public static int Next(int minValue, int maxValue) => _rnd.Next(minValue, maxValue);

        public static void NextBytes(byte[] buffer) => _rnd.NextBytes(buffer);
        public static void NextBytes(Span<byte> buffer) => _rnd.NextBytes(buffer);

        public static double NextDouble() => _rnd.NextDouble();
    }
}
