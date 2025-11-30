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
        [Impl(256)] public static string ToHex(this Color self) => $"#{ToByte(self.r):X2}{ToByte(self.g):X2}{ToByte(self.b):X2}{ToByte(self.a):X2}";
        
        [Impl(256)] public static Color32 ToColor32(this Color self) => new(ToByte(self.r), ToByte(self.g), ToByte(self.b), ToByte(self.a));

        [Impl(256)] public static Color SetAlpha(this Color self, float alpha)
        {
            self.a = alpha;
            return self;
        }

        [Impl(256)] public static bool IsEquals(this Color32 self, Color32 other)
        {
            return self.r == other.r && self.g == other.g && self.b == other.b && self.a == other.a;
        }

        private const float F_MIN_BYTE = byte.MinValue, F_MAX_BYTE = byte.MaxValue;
        private static byte ToByte(float value)
        {
            value = value * F_MAX_BYTE + 0.5f;
            if (value < F_MIN_BYTE) return byte.MinValue;
            if (value > F_MAX_BYTE) return byte.MaxValue;
            return (byte)value;
        }
    }
}
