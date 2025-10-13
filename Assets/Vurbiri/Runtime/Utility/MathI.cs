using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public static class MathI
	{
        [Impl(256)] public static int RoundToInt(float num) => (int)MathF.Round(num);
        [Impl(256)] public static int CeilToInt(float num)  => (int)MathF.Ceiling(num);
        [Impl(256)] public static int FloorToInt(float num) => (int)MathF.Floor(num);


        public static int Pow(int num, int exp)
        {
            int result = 1;
            for (int i = 0; i < exp; i++)
                result *= num;
            return result;
        }

        public static int BinaryPow(int num, int exp)
        {
            int result = 1;
            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result *= num;
                num *= num;
                exp >>= 1;
            }
            return result;
        }
    }
}
