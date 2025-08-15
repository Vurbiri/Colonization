using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class EnumExtensions
    {
        [Impl(256)] public static int ToInt<T>(this T self) where T : Enum => Convert.ToInt32(self);
        [Impl(256)] public static T ToEnum<T>(this int self) where T : Enum => (T)Enum.ToObject(typeof(T), self);
        [Impl(256)] public static T ToEnum<T>(this string self) where T : struct, Enum => Enum.Parse<T>(self);
    }
}
