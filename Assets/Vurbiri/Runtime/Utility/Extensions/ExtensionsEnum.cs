using System;

namespace Vurbiri
{
    public static class ExtensionsEnum
    {
        public static int ToInt<T>(this T self) where T : Enum => Convert.ToInt32(self);
        public static T ToEnum<T>(this int self) where T : Enum => (T)Enum.ToObject(typeof(T), self);
        public static T ToEnum<T>(this string self) where T : struct, Enum => Enum.Parse<T>(self);
    }
}
