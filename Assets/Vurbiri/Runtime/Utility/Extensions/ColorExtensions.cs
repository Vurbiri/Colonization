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
        [Impl(256)] public static string ToHex(this Color self)
        {
            return $"#{MathI.RoundToInt(self.r * 255f):X2}{MathI.RoundToInt(self.g * 255f):X2}{MathI.RoundToInt(self.b * 255f):X2}{MathI.RoundToInt(self.a * 255f):X2}";
        }

        [Impl(256)] public static Color SetAlpha(this Color self, float alpha)
        {
            self.a = alpha;
            return self;
        }

        [Impl(256)] public static bool IsEquals(this Color32 self, Color32 other)
        {
            return self.r == other.r & self.g == other.g & self.b == other.b & self.a == other.a;
        }
    }
}
