using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public static class ColorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Brightness(this Color self, float brightness)
        {
            for (int i = 0; i < 3; i++)
                self[i] = Mathf.Clamp01(self[i] * brightness);

            return self;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHex(this Color32 self) => $"#{self.r:X2}{self.g:X2}{self.b:X2}{self.a:X2}";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHex(this Color self) => ToHex((Color32)self);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color SetAlpha(this Color self, float alpha)
        {
            self.a = alpha;
            return self;
        }
    }
}
