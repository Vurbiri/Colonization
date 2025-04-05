//Assets\Vurbiri\Runtime\Types\EnumT.cs
using System;

namespace Vurbiri
{
    public static class Enum<T> where T : Enum
    {
        public static int Count => Enum.GetValues(typeof(T)).Length;
        public static T[] Values => (T[])Enum.GetValues(typeof(T));
        public static string[] Names => Enum.GetNames(typeof(T));
        public static T Rand(int minInclusive, int maxExclusive) => UnityEngine.Random.Range(minInclusive, maxExclusive).ToEnum<T>();
    }
}
