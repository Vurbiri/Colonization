using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.UI
{
	public class ShaderUIGradient : ShaderGUI
	{
		private const int KEY_COUNT = 8;
		private static readonly string[] s_color = new string[KEY_COUNT], s_colorTime = new string[KEY_COUNT], s_alpha = new string[KEY_COUNT], s_alphaTime = new string[KEY_COUNT];

        static ShaderUIGradient()
		{
            for (int i = 0; i < KEY_COUNT; ++i)
			{
				s_color[i]	   = $"_Color{i}";
				s_colorTime[i] = $"_ColorTime{i}";
                s_alpha[i]     = $"_Alpha{i}";
				s_alphaTime[i] = $"_AlphaTime{i}";
            }
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			base.OnGUI(materialEditor, properties);

			EditorGUILayout.Space();

			var material = (Material)materialEditor.target;

            bool isDirty = false;

            int count = material.GetInt("_Colors");
			var colorKeys = new GradientColorKey[count];
            for (int i = 0; i < count; ++i)
				colorKeys[i] = new(material.GetColor(s_color[i]), material.GetFloat(s_colorTime[i]));

            count = material.GetInt("_Alphas");
            var alphaKeys = new GradientAlphaKey[count];
            for (int i = 0; i < count; ++i)
                alphaKeys[i] = new(material.GetFloat(s_alpha[i]), material.GetFloat(s_alphaTime[i]));

			Gradient gradient = new();
			gradient.SetKeys(colorKeys, alphaKeys);

			EditorGUI.BeginChangeCheck();
			{
				gradient = EditorGUILayout.GradientField("Gradient", gradient);
			}
			if (EditorGUI.EndChangeCheck())
			{
				count = gradient.colorKeys.Length;
                material.SetInt("_Colors", count);
                for (int i = 0; i < count; ++i)
                    SetColorKey(material, i, gradient.colorKeys[i]);
                for (int i = count; i < KEY_COUNT; ++i)
                    SetColor(material, i, Color.white, -1f);

                count = gradient.alphaKeys.Length;
                material.SetInt("_Alphas", count);
                for (int i = 0; i < count; ++i)
                    SetAlphaKey(material, i, gradient.alphaKeys[i]);
                for (int i = count; i < KEY_COUNT; ++i)
                    SetAlpha(material, i, -1f, -1f);

                isDirty = true;
            }

            bool isVertical = material.GetInt("_IsVertical") > 0;
            EditorGUI.BeginChangeCheck();
            {
                isVertical = EditorGUILayout.Toggle("Is Vertical", isVertical);
            }
			if (EditorGUI.EndChangeCheck())
			{
				material.SetInt("_IsVertical", isVertical ? 1 : 0);
                isDirty = true;
            }

            if(isDirty) EditorUtility.SetDirty(material);

            #region Local
            // ===========================================================================
            static void SetColorKey(Material material, int index, GradientColorKey key) => SetColor(material, index, key.color, key.time);
            // ===========================================================================
            static void SetColor(Material material, int index, Color color, float time)
            {
                material.SetColor(s_color[index], color);
                material.SetFloat(s_colorTime[index], time);
            }
            // ===========================================================================
            static void SetAlphaKey(Material material, int index, GradientAlphaKey key) => SetAlpha(material, index, key.alpha, key.time);
            // ===========================================================================
            static void SetAlpha(Material material, int index, float alpha, float time)
            {
                material.SetFloat(s_alpha[index], alpha);
                material.SetFloat(s_alphaTime[index], time);
            }
            // ===========================================================================
            #endregion
        }
    }
}
