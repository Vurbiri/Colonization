using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(InverseAttribute))]
	public class InverseAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            if (attribute is not InverseAttribute inverse || (mainProperty.propertyType != SerializedPropertyType.Float && mainProperty.propertyType != SerializedPropertyType.Integer))
            {
                PropertyField(position, mainProperty, label, true);
                return;
            }

            position.height = EditorGUIUtility.singleLineHeight;

            label = BeginProperty(position, label, mainProperty);
            {
                if (mainProperty.propertyType == SerializedPropertyType.Float)
                {
                    float value = inverse.offsetF - Mathf.Clamp(mainProperty.floatValue, inverse.minF, inverse.maxF);
                    mainProperty.floatValue = inverse.offsetF - Slider(position, label, value, inverse.minF, inverse.maxF);

                }
                else
                {
                    int value = inverse.offsetI - Mathf.Clamp(mainProperty.intValue, inverse.minI, inverse.maxI);
                    mainProperty.intValue = inverse.offsetI - IntSlider(position, label, value, inverse.minI, inverse.maxI);
                }
            }
            EndProperty();
        }

	}
}