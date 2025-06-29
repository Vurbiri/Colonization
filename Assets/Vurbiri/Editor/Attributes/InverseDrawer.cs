using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(InverseAttribute))]
	public class InverseDrawer : PropertyDrawer
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

            if (mainProperty.propertyType == SerializedPropertyType.Float)
            {
                mainProperty.floatValue = Mathf.Clamp(mainProperty.floatValue, inverse.minF, inverse.maxF);
                mainProperty.floatValue = inverse.offsetF - Slider(position, label, inverse.offsetF - mainProperty.floatValue, inverse.minF, inverse.maxF);

            }
            else
            {
                mainProperty.intValue = Mathf.Clamp(mainProperty.intValue, inverse.minI, inverse.maxI);
                mainProperty.intValue = inverse.offsetI - IntSlider(position, label, inverse.offsetI - mainProperty.intValue, inverse.minI, inverse.maxI);
            }
            EndProperty();
        }

	}
}