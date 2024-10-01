namespace Vurbiri
{
    public static class ExtensionsString 
    {
        public static string Concat(this string self, string str) => string.Concat(self, str);
        public static string Concat(this string self, string strA, string strB) => string.Concat(self, strA, strB);
        public static string Concat(this string self, string strA, string strB, string strC) => string.Concat(self, strA, strB, strC);

        public static string Concat(this string self, object obj) => string.Concat(self, obj);
        public static string Concat(this string self, object objA, object objB) => string.Concat(self, objA, objB);
        public static string Concat(this string self, object objA, object objB, object objC) => string.Concat(self, objA, objB, objC);

    }
}
