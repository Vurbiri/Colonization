//Assets\Vurbiri\Editor\UtilityEditor\STYLES.cs
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public static class STYLES
	{
        public static readonly GUIStyle H1;
        public static readonly GUIStyle H2;
        public static readonly GUIStyle border;
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

            border = new()
            {
                name = "border",
                border = new(4, 4, 4, 4),
                padding = new(6, 6, 6, 6)

            };
            border.normal.background = Border(new(66, 66, 66, 255));

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

        public static Texture2D Border(Color32 color, int size = 16, int border = 2)
        {
            int borderMin = border, borderMax = size - border;
            Color32 alpha = new(0, 0, 0, 0);
            Color32[] pixels = new Color32[size * size];

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                     if (i < borderMin | i >= borderMax | j < borderMin | j >= borderMax)
                        pixels[size * i + j] = color;
                    else
                        pixels[size * i + j] = alpha;
                }
            }
            
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
