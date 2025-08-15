using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class ColorExtensions
    {
        [Impl(256)] public static Color Brightness(this Color self, float brightness)
        {
            for (int i = 0; i < 3; i++)
                self[i] = Mathf.Clamp01(self[i] * brightness);

            return self;
        }

        [Impl(256)] public static string ToHex(this Color32 self) => $"#{self.r:X2}{self.g:X2}{self.b:X2}{self.a:X2}";
        [Impl(256)] public static string ToHex(this Color self) => ToHex((Color32)self);

        [Impl(256)] public static Color SetAlpha(this Color self, float alpha)
        {
            self.a = alpha;
            return self;
        }
    }
}
