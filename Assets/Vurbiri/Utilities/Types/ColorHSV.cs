using UnityEngine;

namespace Vurbiri
{
    public struct ColorHSV
    {
        private float _h, _s, _v, _a;

        public float Hue { get => _h; set => _h = value; }
        public float H { get => _h; set => _h = value; }

        public float Saturation { get => _s; set => _s = value; }
        public float S { get => _s; set => _s = value; }

        public float Value { get => _v; set => _v = value; }
        public float V { get => _v; set => _v = value; }

        public float Alfa { get => _a; set => _a = value; }
        public float A { get => _a; set => _a = value; }

        public ColorHSV(Color color)
        {
            Color.RGBToHSV(color, out _h, out _s, out _v);
            _a = color.a;
        }

        public static implicit operator ColorHSV(Color value) => new(value);
        public static implicit operator Color(ColorHSV value)
        {
            Color color = Color.HSVToRGB(value._h, value._s, value._v);
            color.a = value._a;
            return color;
        }
    }
}
