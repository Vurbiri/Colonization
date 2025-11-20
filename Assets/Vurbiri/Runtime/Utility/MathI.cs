using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public static class MathI
	{
        [Impl(256)] public static int Round(float num) => (int)MathF.Round(num);
        [Impl(256)] public static int Ceil(float num)  => (int)MathF.Ceiling(num);
        [Impl(256)] public static int Floor(float num) => (int)MathF.Floor(num);

        [Impl(256)] public static int Sqrt(int num) => (int)MathF.Sqrt(num);
        [Impl(256)] public static int SqrtRound(int num) => (int)(MathF.Sqrt(num) + 0.5f);

        [Impl(256)] public static int Clamp(int value, int min, int max)
        {
            int temp = (min - value) >> 31;
            value = (value & temp) | (min & ~temp);
            temp = (value - max) >> 31;
            return (value & temp) | (max & ~temp);
        }

        [Impl(256)] public static int Min(int x, int y)
        {
            int t = (x - y) >> 31;
            return (x & t) | (y & ~t);
        }
        [Impl(256)] public static int Max(int x, int y)
        {
            int t = (y - x) >> 31;
            return (x & t) | (y & ~t);
        }

        [Impl(256)] public static int Abs(int x)
        {
            int t = x >> 31;
            return (x ^ t) - t;
        }

        public static int Pow(int num, int exp)
        {
            int result = 1;
            while (exp --> 0)
                result *= num;
            return result;
        }

        public static int BinaryPow(int num, int exp)
        {
            int result = 1;
            while (exp > 0)
            {
                result += (exp & 1) * result * (num - 1);
                num *= num;
                exp >>= 1;
            }
            return result;
        }

        [Impl(256)] public static int Equal(int x, int y) => ~((x - y) | (y - x)) >> 31; // x == y ? -1 : 0
        [Impl(256)] public static int NotEqual(int x, int y) => ((x - y) | (y - x)) >> 31; // x != y ? -1 : 0

        [Impl(256)] public static int Less(int x, int y) => (x - y) >> 31; // x < y ? -1 : 0
        [Impl(256)] public static int GreaterOrEqual(int x, int y) => ~(x - y) >> 31; // x >= y ? -1 : 0

        [Impl(256)] public static int Greater(int x, int y) => (y - x) >> 31; // x > y ? -1 : 0
        [Impl(256)] public static int LessOrEqual(int x, int y) => ~(y - x) >> 31; // x <= y ? -1 : 0
    }
}
