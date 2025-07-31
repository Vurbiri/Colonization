using System.Runtime.CompilerServices;

namespace Vurbiri
{
    public static class StringExtensions 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this string self, string str) => string.Concat(self, str);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this string self, string strA, string strB) => string.Concat(self, strA, strB);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this string self, string strA, string strB, string strC) => string.Concat(self, strA, strB, strC);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat<T>(this string self, T obj) => string.Concat(self, obj.ToString());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat<TA, TB>(this string self, TA objA, TB objB) => string.Concat(self, objA.ToString(), objB.ToString());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat<TA, TB, TC>(this string self, TA objA, TB objB, TC objC) => string.Concat(self, objA.ToString(), objB.ToString(), objC.ToString());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Delete(this string self, string str) => self.Replace(str, string.Empty);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Delete(this string self, string strA, string strB) => self.Replace(strA, string.Empty).Replace(strB, string.Empty);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Delete(this string self, params string[] str)
        {
            for (int i = 0; i < str.Length; i++)
                self = self.Replace(str[i], string.Empty);
            
            return self;
        }

    }
}
