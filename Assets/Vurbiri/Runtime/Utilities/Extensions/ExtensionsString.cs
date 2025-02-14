//Assets\Vurbiri\Runtime\Utilities\Extensions\ExtensionsString.cs
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

        public static string Delete(this string self, string str) => self.Replace(str, string.Empty);
        public static string Delete(this string self, string strA, string strB) => self.Replace(strA, string.Empty).Replace(strB, string.Empty);
        public static string Delete(this string self, string strA, string strB, string strC) => self.Replace(strA, string.Empty).Replace(strB, string.Empty).Replace(strC, string.Empty);
        public static string Delete(this string self, params string[] str)
        {
            for (int i = 0; i < str.Length; i++)
                self.Replace(str[i], string.Empty);
            
            return self;
        }

    }
}
