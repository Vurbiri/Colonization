using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(ColorHSV))]
	public class ColorHSVDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty propertyHue		  = mainProperty.FindPropertyRelative(nameof(ColorHSV.h));
			SerializedProperty propertySaturation = mainProperty.FindPropertyRelative(nameof(ColorHSV.s));
			SerializedProperty propertyValue	  = mainProperty.FindPropertyRelative(nameof(ColorHSV.v));
			SerializedProperty propertyAlpha	  = mainProperty.FindPropertyRelative(nameof(ColorHSV.a));
            
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
