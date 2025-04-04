//Assets\Vurbiri\Editor\UtilityEditor\STYLES.cs
using UnityEngine;

namespace VurbiriEditor
{
    public static class STYLES
	{
        public static readonly GUIStyle H1;
        public static readonly GUIStyle H2;

        static STYLES()
		{
            H1 = new()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 15,
            };
            H1.normal.textColor = new(200, 200, 222);
            H1.normal.background = Background(new(20, 40, 62, 255));

            H2 = new()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13,
            };
            H2.normal.textColor = new(120, 180, 222);
        }

        private static Texture2D Background(Color32 color)
        {
            int width = 2, height = 2;
            Color32[] pix = new Color32[width * height];
            
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            Texture2D result = new(width, height);
            result.SetPixels32(pix);
            result.Apply();
            return result;
        }
    }
}
