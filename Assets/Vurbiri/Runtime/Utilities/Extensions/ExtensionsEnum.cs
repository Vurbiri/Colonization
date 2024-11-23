//Assets\Vurbiri\Runtime\Utilities\Extensions\ExtensionsEnum.cs
using System;

namespace Vurbiri
{
    public static class ExtensionsEnum
    {
        public static int ToInt<T>(this T self) where T : Enum => (int)(object)self;
        public static int ToInt<T>(this T self, int offset) where T : Enum => (int)(object)self + offset;
        public static T ToEnum<T>(this int self) where T : Enum => (T)Enum.ToObject(typeof(T), self);
    }
}
