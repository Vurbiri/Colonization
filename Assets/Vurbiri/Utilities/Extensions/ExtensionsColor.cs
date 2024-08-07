using UnityEngine;

namespace Vurbiri
{
    public static class ExtensionsColor
    {
        public static Color Brightness(this Color self, float brightness)
        {
            for (int i = 0; i < 3; i++)
                self[i] = Mathf.Clamp01(self[i] * brightness);

            return self;
        }

        public static Color SetAlpha(this Color self, float alpha)
        {
            self.a = alpha;
            return self;
        }

    }
}
