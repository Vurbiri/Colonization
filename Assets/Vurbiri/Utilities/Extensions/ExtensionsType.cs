using System;

namespace Vurbiri
{
    public static class ExtensionsType
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
    }
}
