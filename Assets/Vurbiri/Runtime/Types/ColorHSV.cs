using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct ColorHSV
    {
        public float h;
        public float s;
        public float v;
        public float a;

        public float Hue { readonly get => h; set => h = value; }
        public float Saturation { readonly get => s; set => s = value; }
        public float Value { readonly get => v; set => v = value; }
        public float Alpha { readonly get => a; set => a = value; }

        public ColorHSV(Color color)
        {
            Color.RGBToHSV(color, out h, out s, out v);
            a = color.a;
        }
        public ColorHSV(float h, float s, float v, float a)
        {
            this.h = h; this.s = s; this.v = v; this.a = a;
        }
        public ColorHSV(float h, float s, float v) : this(h, s, v, 1f) { }

        public static implicit operator ColorHSV(Color value) => new(value);
        public static implicit operator Color(ColorHSV value)
        {
            Color color = Color.HSVToRGB(value.h, value.s, value.v);
            color.a = value.a;
            return color;
        }
    }
}
