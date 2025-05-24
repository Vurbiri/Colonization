using System;

namespace Vurbiri
{
    public static class Enum<T> where T : Enum
    {
        public static T[] Values => (T[])Enum.GetValues(typeof(T));
        public static string[] Names => Enum.GetNames(typeof(T));

    }
}
