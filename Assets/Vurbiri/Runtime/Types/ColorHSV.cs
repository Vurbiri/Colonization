using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [System.Serializable]
    public struct ColorHSV
    {
        public float h; // Hue
        public float s; // Saturation
        public float v; // Value
        public float a; // Alpha

        [Impl(256)] public ColorHSV(Color color)
        {
            Color.RGBToHSV(color, out h, out s, out v);
            a = color.a;
        }
        [Impl(256)] public ColorHSV(float h, float s, float v, float a)
        {
            this.h = h; this.s = s; this.v = v; this.a = a;
        }
        [Impl(256)] public ColorHSV(float h, float s, float v) : this(h, s, v, 1f) { }

        [Impl(256)] public static implicit operator ColorHSV(Color value) => new(value);
        [Impl(256)] public static implicit operator Color(ColorHSV value)
        {
            Color color = Color.HSVToRGB(value.h, value.s, value.v);
            color.a = value.a;
            return color;
        }
    }
}
