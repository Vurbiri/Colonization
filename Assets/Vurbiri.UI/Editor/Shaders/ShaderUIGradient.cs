using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VurbiriEditor.UI
{
	public class ShaderUIGradient : ShaderGUI
	{
		private const int KEY_COUNT = 8;
		private static readonly int[] s_colorsID = new int[KEY_COUNT], s_colorsTime = new int[KEY_COUNT], s_alphasID = new int[KEY_COUNT], s_alphasTimeID = new int[KEY_COUNT];
        private static readonly int s_colorsCount = Shader.PropertyToID("_Colors"), s_alphasCount = Shader.PropertyToID("_Alphas");

        static ShaderUIGradient()
		{
			for (int i = 0; i < KEY_COUNT; ++i)
			{
				s_colorsID[i]	  = Shader.PropertyToID($"_Color{i}");
				s_colorsTime[i]   = Shader.PropertyToID($"_ColorTime{i}");
				s_alphasID[i]     = Shader.PropertyToID($"_Alpha{i}");
				s_alphasTimeID[i] = Shader.PropertyToID($"_AlphaTime{i}");
			}
		}

        private Material _material;
        private Gradient _gradient = new();

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			base.OnGUI(materialEditor, properties);

			EditorGUILayout.Space();

			var material = (Material)materialEditor.target;
			var keyVertical = new LocalKeyword(material.shader, "IS_VERTICAL");

			bool isDirty = false;

			if (_material != material)
			{
                _material = material;

                int count = material.GetInt(s_colorsCount);
				var colorKeys = new GradientColorKey[count];
				for (int i = 0; i < count; ++i)
					colorKeys[i] = new(material.GetColor(s_colorsID[i]), material.GetFloat(s_colorsTime[i]));

				count = material.GetInt(s_alphasCount);
				var alphaKeys = new GradientAlphaKey[count];
				for (int i = 0; i < count; ++i)
					alphaKeys[i] = new(material.GetFloat(s_alphasID[i]), material.GetFloat(s_alphasTimeID[i]));

                _gradient.SetKeys(colorKeys, alphaKeys);
			}

			EditorGUI.BeginChangeCheck();
			{
				_gradient = EditorGUILayout.GradientField("Gradient", _gradient);
			}
			if (EditorGUI.EndChangeCheck())
			{
                int count = _gradient.colorKeys.Length;
				material.SetInt(s_colorsCount, count);
				for (int i = 0; i < count; ++i)
					SetColorKey(material, i, _gradient.colorKeys[i]);
				for (int i = count; i < KEY_COUNT; ++i)
					SetColor(material, i, Color.white, -1f);

				count = _gradient.alphaKeys.Length;
				material.SetInt(s_alphasCount, count);
				for (int i = 0; i < count; ++i)
					SetAlphaKey(material, i, _gradient.alphaKeys[i]);
				for (int i = count; i < KEY_COUNT; ++i)
					SetAlpha(material, i, -1f, -1f);

				isDirty = true;
			}

			bool isVertical = material.IsKeywordEnabled(keyVertical);
			EditorGUI.BeginChangeCheck();
			{
				isVertical = EditorGUILayout.Toggle("Is Vertical", isVertical);
			}
			if (EditorGUI.EndChangeCheck())
			{
				material.SetKeyword(keyVertical, isVertical);
				isDirty = true;
			}

			if(isDirty) EditorUtility.SetDirty(material);

			#region Local
			// ===========================================================================
			static void SetColorKey(Material material, int index, GradientColorKey key) => SetColor(material, index, key.color, key.time);
			// ===========================================================================
			static void SetColor(Material material, int index, Color color, float time)
			{
				material.SetColor(s_colorsID[index], color);
				material.SetFloat(s_colorsTime[index], time);
			}
			// ===========================================================================
			static void SetAlphaKey(Material material, int index, GradientAlphaKey key) => SetAlpha(material, index, key.alpha, key.time);
			// ===========================================================================
			static void SetAlpha(Material material, int index, float alpha, float time)
			{
				material.SetFloat(s_alphasID[index], alpha);
				material.SetFloat(s_alphasTimeID[index], time);
			}
			// ===========================================================================
			#endregion
		}
	}
}
