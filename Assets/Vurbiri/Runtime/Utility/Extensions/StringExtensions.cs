namespace Vurbiri
{
    public static class StringExtensions 
    {
        public static string Concat(this string self, string str) => string.Concat(self, str);
        public static string Concat(this string self, string strA, string strB) => string.Concat(self, strA, strB);
        public static string Concat(this string self, string strA, string strB, string strC) => string.Concat(self, strA, strB, strC);

        public static string Concat<T>(this string self, T obj) => string.Concat(self, obj.ToString());
        public static string Concat<TA, TB>(this string self, TA objA, TB objB) => string.Concat(self, objA.ToString(), objB.ToString());
        public static string Concat<TA, TB, TC>(this string self, TA objA, TB objB, TC objC) => string.Concat(self, objA.ToString(), objB.ToString(), objC.ToString());

        public static string Delete(this string self, string str) => self.Replace(str, string.Empty);
        public static string Delete(this string self, string strA, string strB) => self.Replace(strA, string.Empty).Replace(strB, string.Empty);
        public static string Delete(this string self, params string[] str)
        {
            for (int i = 0; i < str.Length; i++)
                self = self.Replace(str[i], string.Empty);
            
            return self;
        }

    }
}
