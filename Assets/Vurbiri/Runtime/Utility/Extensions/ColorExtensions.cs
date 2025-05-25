using UnityEngine;

namespace Vurbiri
{
    public static class ColorExtensions
    {
        public static Color Brightness(this Color self, float brightness)
        {
            for (int i = 0; i < 3; i++)
                self[i] = Mathf.Clamp01(self[i] * brightness);

            return self;
        }

        public static string ToHex(this Color32 self) => $"#{self.r:X2}{self.g:X2}{self.b:X2}{self.a:X2}";
        public static string ToHex(this Color self)
        {
            Color32 temp = self;
            return $"#{temp.r:X2}{temp.g:X2}{temp.b:X2}{temp.a:X2}";
        }

        public static Color SetAlpha(this Color self, float alpha)
        {
            self.a = alpha;
            return self;
        }

    }
}
