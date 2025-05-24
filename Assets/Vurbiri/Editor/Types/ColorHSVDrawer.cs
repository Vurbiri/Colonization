using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(ColorHSV))]
	public class ColorHSVDrawer : PropertyDrawer
	{
		#region Consts
		private const string F_HUE = "h", F_SATURATION = "s", F_VALUE = "v", F_ALPHA = "a";
		#endregion
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty propertyHue		  = mainProperty.FindPropertyRelative(F_HUE);
			SerializedProperty propertySaturation = mainProperty.FindPropertyRelative(F_SATURATION);
			SerializedProperty propertyValue	  = mainProperty.FindPropertyRelative(F_VALUE);
			SerializedProperty propertyAlpha	  = mainProperty.FindPropertyRelative(F_ALPHA);
            
			Color color = Color.HSVToRGB(propertyHue.floatValue, propertySaturation.floatValue, propertyValue.floatValue).SetAlpha(propertyAlpha.floatValue);

            label = BeginProperty(position, label, mainProperty);
            {
				BeginChangeCheck();
					color = ColorField(position, label, color);
				if (EndChangeCheck())
				{
                    Color.RGBToHSV(color, out float h, out float s, out float v);
                    propertyHue.floatValue		  = h;
					propertySaturation.floatValue = s;
					propertyValue.floatValue	  = v;
					propertyAlpha.floatValue	  = color.a;
                }
			}
			EndProperty();
		}

	}
}
