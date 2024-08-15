namespace Vurbiri
{
    public static class ExtensionsString 
    {
        public static string Concat(this string self, string str) => string.Concat(self, str);
        public static string Concat(this string self, string strA, string strB) => string.Concat(self, strA, strB);
        public static string Concat(this string self, string strA, string strB, string strC) => string.Concat(self, strA, strB, strC);

    }
}
