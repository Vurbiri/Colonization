using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public static class STYLES
	{
        public static readonly GUIStyle H1;
        public static readonly GUIStyle H2;
        public static readonly GUIStyle borderLight;
        public static readonly GUIStyle borderDark;
        public static readonly GUIStyle flatButton;

        static STYLES()
		{
            H1 = new()
            {
                name = "H1",
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 15,
            };
            H1.normal.textColor = new(200, 200, 222);
            H1.normal.background = BackgroundColor(new(20, 40, 62, 255));

            H2 = new()
            {
                name = "H2",
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13,
            };
            H2.normal.textColor = new(120, 180, 222);

            borderLight = new()
            {
                name = "borderLight",
                border = new(4, 4, 4, 4),
                padding = new(12, 6, 6, 6)

            };
            borderLight.normal.background = Border(new Color32(77, 77, 77, 255));

            borderDark = new(borderLight)
            {
                name = "borderDark"
            };
            borderDark.normal.background = Border(new Color32(33, 33, 33, 255), new Color32(52, 52, 52, 255));

            flatButton = new()
            {
                name = "flatButton",
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = EditorGUIUtility.singleLineHeight,
                fixedWidth = EditorGUIUtility.singleLineHeight
            };
            flatButton.normal.background = BackgroundColor(new(56, 56, 56, 255));
            flatButton.hover.background = BackgroundColor(new(88, 88, 88, 255));
            flatButton.focused.background = BackgroundColor(new(118, 118, 118, 255));
            flatButton.active.background = BackgroundColor(new(38, 38, 38, 255));
        }

        public static Texture2D BackgroundColor(Color32 color)
        {
            int size = 2, length = size * size;
            Color32[] pixels = new Color32[length];
            
            for (int i = 0; i < length; ++i)
                pixels[i] = color;

            return pixels.ToTexture(size);
        }

        public static Texture2D Border(Color32 colorBorder, int size = 8, int border = 1)
        {
            int borderMin = border, borderMax = size - border;
            Color32[] pixels = new Color32[size * size];

            for (int i = 0; i < borderMin; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMax; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = 0; j < borderMin; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = borderMax; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            return pixels.ToTexture(size);
        }

        public static Texture2D Border(Color32 colorBorder, Color32 colorMain, int size = 8, int border = 1)
        {
            int borderMin = border, borderMax = size - border;
            Color32[] pixels = new Color32[size * size];

            for (int i = 0; i < borderMin; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMax; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = 0; j < borderMin; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = borderMax; j < size; ++j)
                    pixels[size * i + j] = colorBorder;

            for (int i = borderMin; i < borderMax; ++i)
                for (int j = borderMin; j < borderMax; ++j)
                    pixels[size * i + j] = colorMain;
            
            return pixels.ToTexture(size);
        }

        private static Texture2D ToTexture(this Color32[] pixels, int size)
        {
            Texture2D texture = new(size, size);
            texture.SetPixels32(pixels);
            texture.Apply();

            return texture;
        }
    }
}
