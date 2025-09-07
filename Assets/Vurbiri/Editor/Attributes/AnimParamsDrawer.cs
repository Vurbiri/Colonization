using UnityEditor;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
	[CustomPropertyDrawer(typeof(AnimParamsAttribute))]
	public class AnimParamsDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            if (attribute is not AnimParamsAttribute list || mainProperty.propertyType != SerializedPropertyType.Integer)
            {
                PropertyField(position, mainProperty, label, true);
                return;
            }

            label = BeginProperty(position, label, mainProperty);
			{
                mainProperty.intValue = IntPopup(position, label.text, mainProperty.intValue, list.names, list.values);
			}
			EndProperty();
		}
	}
}