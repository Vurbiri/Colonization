using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.UTILITY_MENU_NAME + "FPS/Graph", VUI_CONST_ED.UTILITY_MENU_ORDER)]
    [RequireComponent(typeof(RawImage))]
#endif
    public class FPSGraph : MonoBehaviour
    {
        [SerializeField] private int _width = 128;
        [SerializeField] private int _height = 64;
        [SerializeField] private FilterMode _filterMode = FilterMode.Bilinear;
        [Space]
        [SerializeField] private Color32 _colorBackground = Color.white;
        [SerializeField] private Color32 _colorGraph = Color.black;
        [SerializeField] private Color32 _colorAvg = Color.blue;
        [SerializeField] private Color32 _colorMax = Color.green;
        [SerializeField] private Color32 _colorMin = Color.red;

        private Color32[] _pixels;
        private Texture2D _texture;

        public int Width { [Impl(256)] get => _width; }

        private void Awake()
        {
            _pixels = new Color32[_width * _height];
            _texture = new(_width, _height)
            {
                name = "FPSGraph",
                filterMode = _filterMode
            };

            var image = GetComponent<RawImage>();
            image.color = Color.white;
            image.texture = _texture;
        }

        public void UpdateTexture(IEnumerable<int> values, float fpsAvg, int fpsMax, int fpsMin)
        {
            if (_texture == null)
                return;

            float scale = _height / (fpsMax * 1.15f);
            int x = 0, y, yAvg = MathI.Round(fpsAvg * scale), yMax = MathI.Round(fpsMax * scale), yMin = MathI.Round(fpsMin * scale);

            foreach (int value in values)
            {
                for (y = 0; y < _height; ++y)
                    SetPixel(x, y, _colorBackground);

                y = MathI.Round(value * scale);
                SetPixel(x, y, _colorGraph);
                SetPixel(x, yAvg, _colorAvg);
                SetPixel(x, yMax, _colorMax);
                SetPixel(x, yMin, _colorMin);

                ++x;
            }

            for (; x < _width; ++x)
            {
                for (y = 0; y < _height; ++y)
                    SetPixel(x, y, _colorBackground);

                SetPixel(x, yAvg, _colorAvg);
                SetPixel(x, yMax, _colorMax);
                SetPixel(x, yMin, _colorMin);
            }

            _texture.SetPixels32(_pixels);
            _texture.Apply();
        }

        [Impl(256)] private void SetPixel(int x, int y, Color32 color) => _pixels[x + y * _width] = color;
    }
}
