using System;

namespace Vurbiri
{
    public static class TypeExtensions
    {
        public static bool Is(this Type self, Type other)
        {
            while (self != null)
            {
                if (self == other)
                    return true;

                self = self.BaseType;
            }

            return false;
        }

        public static bool Is(this Type self, Type other, Type deep)
        {
            while (self != null & self != deep)
            {
                if (self == other)
                    return true;

                self = self.BaseType;
            }

            return false;
        }
    }
}
