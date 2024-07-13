using UnityEngine;

public static class ExtensionsColor
{
    public static Color Brightness(this Color self, float brightness)
    {
        for (int i = 0; i < 3; i++)
            self[i] = Mathf.Clamp01(self[i] * brightness);

        self.a = 1f;
        return self;
    }

    public static Color SetAlpha(this Color self, float alpha)
    {
        self.a = alpha;
        return self;
    }

    public static void SetRandMono(this ref Color32 self, MinMaxInt range) => self.r = self.g = self.b = (byte)range.RandIn;
}
