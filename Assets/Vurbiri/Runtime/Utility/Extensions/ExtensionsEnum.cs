//Assets\Vurbiri\Runtime\Utility\Extensions\ExtensionsEnum.cs
using System;

namespace Vurbiri
{
    public static class ExtensionsEnum
    {
        public static int ToInt<T>(this T self) where T : Enum => Convert.ToInt32(self);
        //public static int ToInt<T>(this T self) where T : Enum => (int)(object)self;
        public static T ToEnum<T>(this int self) where T : Enum => (T)Enum.ToObject(typeof(T), self);
    }
}
