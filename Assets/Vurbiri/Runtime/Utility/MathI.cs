using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public static class MathI
	{
        [Impl(256)] public static int Round(this float num) => (int)MathF.Round(num);
        [Impl(256)] public static int Ceil(this float num)  => (int)MathF.Ceiling(num);
        [Impl(256)] public static int Floor(this float num) => (int)MathF.Floor(num);

        [Impl(256)] public static int Sqrt(this int num) => (int)MathF.Sqrt(num);
        [Impl(256)] public static int SqrtRound(this int num) => (int)(MathF.Sqrt(num) + 0.5f);

        [Impl(256)] public static int Clamp(this int num, int min, int max)
        {
            int mask = (min - num) >> 31;
            num = (num & mask) | (min & ~mask);
            mask = (num - max) >> 31;
            return (num & mask) | (max & ~mask);
        }

        [Impl(256)] public static int Min(int x, int y)
        {
            int m = (x - y) >> 31;
            return (x & m) | (y & ~m);
        }
        [Impl(256)] public static int Max(int x, int y)
        {
            int m = (y - x) >> 31;
            return (x & m) | (y & ~m);
        }

        [Impl(256)] public static int Abs(this int x)
        {
            int m = x >> 31;
            return (x ^ m) - m;
        }

        [Impl(256)] public static int SetRow(this int num, int row, bool isOne) => isOne ? num | (1 << row) : num & ~(1 << row);

        public static int Pow(this int num, int exp)
        {
            int result = 1;
            while (exp --> 0)
                result *= num;
            return result;
        }

        public static int BinaryPow(this int num, int exp)
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

        [Impl(256)] public static int Select(bool isX, int x, int y)
        {
            int mask = Convert.ToInt32(isX) - 1;
            return (x & ~mask) | (y & mask);
        }
    }
}
