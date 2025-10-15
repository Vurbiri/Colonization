using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class SysRandom
	{
        private static readonly Random s_rnd = new();
        private static readonly byte[] s_longBuffer = new byte[8];

        [Impl(256)] public static int Next() => s_rnd.Next();
        [Impl(256)] public static int Next(int maxExclusive) => s_rnd.Next(maxExclusive);
        [Impl(256)] public static int Next(int minInclusive, int maxExclusive) => s_rnd.Next(minInclusive, maxExclusive);

        [Impl(256)] public static void NextBytes(byte[] buffer) => s_rnd.NextBytes(buffer);
        [Impl(256)] public static void NextBytes(Span<byte> buffer) => s_rnd.NextBytes(buffer);

        [Impl(256)] public static double NextDouble() => s_rnd.NextDouble();

        [Impl(256)] public static uint Next(uint maxExclusive) => (uint)(s_rnd.Next(int.MinValue, (int)(maxExclusive + int.MinValue)) - int.MinValue);
        [Impl(256)] public static uint Next(uint minInclusive, uint maxExclusive)
        {
            return (uint)(s_rnd.Next((int)(minInclusive + int.MinValue), (int)(maxExclusive + int.MinValue)) - int.MinValue);
        }

        public static ulong Next(ulong maxExclusive)
        {
            s_rnd.NextBytes(s_longBuffer);
            return BitConverter.ToUInt64(s_longBuffer) % maxExclusive;
        }
    }
}
